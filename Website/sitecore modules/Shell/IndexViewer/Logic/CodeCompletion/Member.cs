using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace IndexViewer.sitecore_modules.Shell.IndexViewer.Logic.CodeCompletion
{
    public class Member
    {
        private System.Reflection.MemberInfo _memberInfo;

        public Member(System.Reflection.MemberInfo memberInfo)
        {
            this._memberInfo = memberInfo;
        }

        public MemberTypes MemberType
        {
            get
            {
                return _memberInfo.MemberType;
            }
        }

        public string MemberName
        {
            get
            {
                return _memberInfo.Name;
            }
        }

        public string DisplayMemberName
        {
            get
            {
                return MemberName;
            }
        }
    }
}
