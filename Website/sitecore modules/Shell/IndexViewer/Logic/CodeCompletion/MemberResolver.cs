using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Web;

namespace IndexViewer.sitecore_modules.Shell.IndexViewer.Logic.CodeCompletion
{
    public class MemberResolver
    {

        public IEnumerable<Member> GetMembersForCodeCompletion(string fullyQualifiedType)
        {
            IEnumerable<Member> allMembers = GetMembers(fullyQualifiedType);
            var membersToShow = FilterSetters(allMembers);
            return membersToShow;
        }

        private IEnumerable<Member> FilterSetters(IEnumerable<Member> allMembers)
        {
            foreach (var member in allMembers)
            {
                if (member.MemberName.StartsWith("set_") || member.MemberName.StartsWith("get_"))
                    continue;

                if(member.MemberName.StartsWith(".ctor"))
                {
                    continue;    
                }

                yield return member;
            }
        }

        public IEnumerable<Member> GetMembers(string fullyQualifiedType)
        {
            try
            {
                var searchType = Type.GetType(fullyQualifiedType);
                var members = searchType.GetMembers(BindingFlags.Instance | BindingFlags.Public);
                return members.Select(m => new Member(m));
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException("Invalid Search type: " + fullyQualifiedType);
            }
        }
    }
}