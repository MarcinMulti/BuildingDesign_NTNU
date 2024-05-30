using System;
using System.Drawing;
using Grasshopper;
using Grasshopper.Kernel;

namespace MasterThesisHege
{
    public class MasterThesisHegeInfo : GH_AssemblyInfo
    {
        public override string Name => "MasterThesisHege";
        /*
        //Return a 24x24 pixel bitmap to represent this GHA library.
        public override Bitmap Icon => null;
        */

        //Return a short string describing the purpose of this GHA library.
        public override string Description => "";

        public override Guid Id => new Guid("9206db84-8cf6-486e-990e-32a7a4b7e259");

        //Return a string identifying you or your company.
        public override string AuthorName => "";

        //Return a string representing your preferred contact details.
        public override string AuthorContact => "";
    }
}