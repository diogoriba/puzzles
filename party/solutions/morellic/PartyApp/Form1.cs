using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using ChallengeParty;

namespace PartyApp
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Tree tree = RequestParsing.GetTreeFromUrl("https://s3-sa-east-1.amazonaws.com/public-deploy/codingchallenge/party/tree.json");
            tree.ProcessTree();

            Party party = new Party(tree.PersonTree);

            for (int i = 1; i < tree.tree.Length; i++)
            {
                party.ComputeGroupsBySize(i);

                if (party.GroupsBySize[i] == null || party.GroupsBySize[i].Count == 0)
                {
                    break;
                }
            }

            // Now we just need to traverse the party.GroupsBySize structure and take note of the coolest groups
            long coolestValue = 0;

            List<PersonGroup> coolestGroups = new List<PersonGroup>();

            foreach (List<PersonGroup> listGroups in party.GroupsBySize.Values)
            {
                foreach (PersonGroup group in listGroups)
                {
                    if (group.CoolnessNumber > coolestValue)
                    {
                        coolestGroups.Clear();
                        coolestGroups.Add(group);
                        coolestValue = group.CoolnessNumber;
                    }
                    else if (group.CoolnessNumber == coolestValue)
                    {
                        coolestGroups.Add(group);
                    }
                }
            }


        }
    }
}
