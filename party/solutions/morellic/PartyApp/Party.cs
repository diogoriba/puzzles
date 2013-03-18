using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ChallengeParty;

namespace PartyApp
{
    public class Party
    {
        public Dictionary<string, Person> PersonHierarchy { get; private set; }

        public Dictionary<int, List<PersonGroup>> GroupsBySize { get; set; }
        public Dictionary<string, List<PersonGroup>> GroupsByPerson { get; set; }

        public Party(Dictionary<string, Person> hierarchy)
        {
            this.PersonHierarchy = hierarchy;
            this.GroupsByPerson = new Dictionary<string, List<PersonGroup>>();
            this.GroupsBySize = new Dictionary<int, List<PersonGroup>>();
        }

        private void ComputeSizeOne()
        {
            List<PersonGroup> listGroupsBySize = new List<PersonGroup>();

            this.GroupsBySize.Add(1, listGroupsBySize);

            foreach (Person p in PersonHierarchy.Values)
            {
                List<PersonGroup> listGroupsByPerson = new List<PersonGroup>();
                this.GroupsByPerson.Add(p.id, listGroupsByPerson);

                PersonGroup newGroup = new PersonGroup() { InGroup = new List<Person> { p } };
                if (p.Parent != null)
                {
                    newGroup.NotAllowed.Add(p.Parent);
                }

                if (p.ChildrenList != null)
                {
                    newGroup.NotAllowed.AddRange(p.ChildrenList);
                }

                newGroup.LastId = p.id;

                listGroupsBySize.Add(newGroup);
                listGroupsByPerson.Add(newGroup);
            }
        }

        public void ComputeGroupsBySize(int size)
        {
            if (size >= 1 && size <= PersonHierarchy.Count)
            {
                if (size == 1)
                {
                    this.ComputeSizeOne();
                }

                if (size > 1)
                {
                    if (!this.GroupsBySize.ContainsKey(size - 1))
                    {
                        this.ComputeGroupsBySize(size - 1);
                    }

                    List<PersonGroup> listGroups = this.GroupsBySize[size - 1];

                    this.GroupsBySize.Add(size, new List<PersonGroup>());

                    foreach (Person p in PersonHierarchy.Values)
                    {
                        foreach (PersonGroup group in listGroups)
                        {
                            if ((group.LastId.CompareTo(p.id) < 0))
                                //&& !this.GroupsByPerson[p.id].Contains(group))
                            {
                                PersonGroup newGroup = group.AddMemberToGroup(p);
                                if (newGroup != null)
                                {
                                    this.GroupsBySize[size].Add(newGroup);

                                    foreach (Person pp in newGroup.InGroup)
                                    {
                                        this.GroupsByPerson[pp.id].Add(newGroup);
                                    }
                                }
                            }
                        }
                    }
                }
            }
        }
    }

    public class PersonGroup
    {
        public PersonGroup()
        {
            this.InGroup = new List<Person>();
            this.NotAllowed = new List<Person>();
        }

        public string LastId { get; set; }

        public List<Person> InGroup { get; set; }

        public int Size { get { return this.InGroup != null ? this.InGroup.Count : 0; } }

        public List<Person> NotAllowed { get; set; }

        public long CoolnessNumber
        {
            get
            {
                long result = 0;
                if (this.InGroup != null)
                {
                    foreach (Person p in InGroup)
                    {
                        result += p.CoolnessNumber;
                    }
                }

                return result;
            }
        }

        public PersonGroup AddMemberToGroup(Person newMember)
        {
            if (this.LastId.CompareTo(newMember.id) > 0)
            {
                // In order to avoid unecessary combinations
                return null;
            }

            if (this.NotAllowed.Contains(newMember) 
                || this.InGroup.Contains(newMember)
                || this.InGroup.Contains(newMember.Parent))
            {
                return null;
            }

            PersonGroup newGroup = new PersonGroup();
            newGroup.InGroup.AddRange(this.InGroup);
            newGroup.NotAllowed.AddRange(this.NotAllowed);

            newGroup.InGroup.Add(newMember);

            if (newMember.ChildrenList != null)
            {
                newGroup.NotAllowed.AddRange(newMember.ChildrenList);
            }

            if (newMember.Parent != null)
            {
                newGroup.NotAllowed.Add(newMember.Parent);
            }

            newGroup.LastId = newMember.id;

            return newGroup;
        }
    }
}
