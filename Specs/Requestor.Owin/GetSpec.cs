using System;
using System.Collections.Generic;
using Machine.Specifications;
using Requestoring;

namespace Requestoring.Specs {

    [Subject(typeof(Requestor))]
    public class Get_using_OWIN : OwinSpec {
        Behaves_like<Get_behaviors> get_stuff;
    }
}
