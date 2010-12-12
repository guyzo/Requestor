using System;
using System.Collections.Generic;
using Machine.Specifications;
using Requestoring;

namespace Requestoring.Specs {

    [Subject(typeof(Requestor))]
    public class Get_using_OWIN : OwinSpec {
        Behaves_like<Get_behaviors> behaviors;
    }

    [Subject(typeof(Requestor))][SetupForEachSpecification]
    public class SettingHeaders_using_OWIN : OwinSpec {
	Behaves_like<SettingHeaders_behaviors> behaviors;
    }

    [Subject(typeof(Requestor))]
    public class Post_using_OWIN : OwinSpec {
	Behaves_like<Post_behaviors> behaviors;
    }

    // Need to move the post variable stuff into Requestor base ... it was silly to put it into HttpRequestor
    //[Subject(typeof(Requestor))][SetupForEachSpecification]
    //public class Put_using_OWIN : OwinSpec {
    //    Behaves_like<Put_behaviors> behaviors;
    //}

    //[Subject(typeof(Requestor))][SetupForEachSpecification]
    //public class Delete_using_OWIN : OwinSpec {
    //    Behaves_like<Delete_behaviors> behaviors;
    //}
}
