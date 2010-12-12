using System;
using System.Collections.Generic;
using Machine.Specifications;
using Requestoring;

namespace Requestoring.Specs {

    [Subject(typeof(Requestor))]
    public class Get_owin : OwinSpec {
        Behaves_like<Get_behaviors> get_stuff;
    }
}