using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace ChallengeParty
{
    [DataContract]
    public class Tree
    {
        [DataMember]
        public Person[] tree { get; set; }

        public Dictionary<string, Person> PersonTree { get; set; }

        public void ProcessTree()
        {
            if (this.tree != null)
            {
                this.PersonTree = new Dictionary<string, Person>();
                foreach (Person p in tree)
                {
                    this.PersonTree.Add(p.id, p);
                }

                foreach (Person p in tree)
                {
                    if (p.children != null && p.children.Length > 0)
                    {
                        p.ChildrenList = new List<Person>();

                        foreach (string cId in p.children)
                        {
                            if (this.PersonTree.ContainsKey(cId))
                            {
                                p.ChildrenList.Add(this.PersonTree[cId]);
                                this.PersonTree[cId].Parent = p;
                            }
                        }
                    }

                }
            }
        }


    }

    [DataContract]
    public class Person
    {
        [DataMember]
        public string id { get; set; }
        [DataMember]
        public string coolness { get; set; }
        [DataMember]
        public string gender { get; set; }
        [DataMember]
        public string[] children { get; set; }
        
        public int CoolnessNumber { get { int coolnum = 0; Int32.TryParse(coolness, out coolnum); return coolnum; } }

        public Person Parent { get; set; }

        public List<Person> ChildrenList { get; set; }
    }
}
