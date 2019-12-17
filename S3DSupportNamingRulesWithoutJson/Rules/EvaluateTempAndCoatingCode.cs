using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace S3DNamingRules.Rules
{
    public class EvaluateTempAndCoatingCode
    {
        public List<string> GetTempCoatingCode(double minDesignTemperature, double maxDesignTemperature, string materialClass)
        {
            string tempcode = String.Empty;
            string coatingCode = string.Empty;


            if ((materialClass == "Galavanized CS")|| (materialClass == "12"))
            {
                if ((minDesignTemperature >= -48) && (minDesignTemperature <= -11))
                {

                    tempcode = "T6";
                    coatingCode = "G";

                }

                if ((minDesignTemperature >= -10) && (minDesignTemperature <= 120))
                {
                    tempcode = "T1";
                    coatingCode = "G";

                }

            }

            if (materialClass == "Carbon Steel")
            {

                if ((minDesignTemperature >= 238.15) && (minDesignTemperature <= 262.15))
                {

                    tempcode = "T6";
                    coatingCode = "Z";

                }

                if ((minDesignTemperature >= 263.15) && (minDesignTemperature <= 393.15))
                {
                    tempcode = "T1";
                    coatingCode = "Z";

                }

                if ((minDesignTemperature >= 394.15) && (minDesignTemperature <= 473.15))
                {
                    tempcode = "T1";
                    coatingCode = "F";

                }

                if ((minDesignTemperature >= 474.15) && (minDesignTemperature <= 573.15))
                {
                    tempcode = "T1";
                    coatingCode = "S";

                }
                if ((minDesignTemperature >= 574.15) && (minDesignTemperature <= 723.15))
                {
                    tempcode = "T2";
                    coatingCode = "S";

                }
                if ((minDesignTemperature >= 724.15) && (minDesignTemperature <= 773.15))
                {
                    tempcode = "T2";
                    coatingCode = "A";

                }
                if ((minDesignTemperature >= 774.15) && (minDesignTemperature <= 813.15))
                {
                    tempcode = "T3";
                    coatingCode = "A";

                }
            }

            List<string> codes = new List<string>();
            codes.Add(tempcode);
            codes.Add(coatingCode);
            //Codes codes = new Codes();
            //codes.tempCode = tempcode;
            //codes.coatingCode = coatingCode;
           return codes;

            // return Tuple.Create(tempcode, coatingCode);


        }


    }
}
