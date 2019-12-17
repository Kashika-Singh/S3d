using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ingr.SP3D.Common.Middle;
using Ingr.SP3D.Support.Middle;
using Ingr.SP3D.Common.Middle.Services;


namespace S3DNamingRules.Rules
{
    public class S3DPipeSupportNameRule :NameRuleBase
    {
        const string NAME_INTERFACE = "IJNamedItem";
        const string NAME_PROPERTY = "Name";



        public override void ComputeName(BusinessObject oEntity, ReadOnlyCollection<BusinessObject> oParents, BusinessObject oActiveEntity)
        {
            try
            {
                Ingr.SP3D.Support.Middle.Support oHgrSupport = (Ingr.SP3D.Support.Middle.Support)oEntity;
                IPart oSupportDefinition = oHgrSupport.SupportDefinition;
                //Get the part class name of support
                var partNumber = oSupportDefinition.PartNumber;
                
                var supportName = "undefined";
              
                //Get supported and supporting objects count

                //var pipeObjectCount = oHgrSupport.SupportedObjects.Count();
                var structureObjectCount = oHgrSupport.SupportingObjects.Count();
                var width = 0.0;
                string temperatureCodeValue = "Undefined";
                string coatingCodeValue = "Undefined";
                //Check if pipeObject 
                //   if (pipeObjectCount!=0)
                //  {

                //Created Helper class object to get pipeobject and beam object information
                SupportedHelper supportHelper = new SupportedHelper(oHgrSupport);
                Ingr.SP3D.Support.Middle.PipeObjectInfo pipeObject = supportHelper.SupportedObjectInfo(1) as Ingr.SP3D.Support.Middle.PipeObjectInfo;
                double nominalDiameter = pipeObject.NominalDiameter.Size;
                double minDesignTemperature = pipeObject.MinDesignTemperature;
                double maxDesignTemperature = pipeObject.MaxDesignTemperature;

                //Get Material Class property
                BusinessObject routeObject = oHgrSupport.SupportedObjects[0];
                RelationCollection oRel1 = routeObject.GetRelationship("PathSpecification", "thePathSystemInfo");
                BusinessObject oRun = oRel1.TargetObjects[0];
                RelationCollection oRel2 = oRun.GetRelationship("PathRunUsesSpec", "Spec");
                string materialClass = oRel2.TargetObjects[0].GetPropertyValue("IJDPipeSpec", "PipingNote1").ToString();


               // string materialClass = "Carbon Steel";
               //Get 
                EvaluateTempAndCoatingCode evaluateTempAndCoatingCode = new EvaluateTempAndCoatingCode();
                List<string> codes = evaluateTempAndCoatingCode.GetTempCoatingCode(minDesignTemperature, maxDesignTemperature, materialClass);
                
                foreach (string val in codes)
                {
                    temperatureCodeValue = codes[0].ToString();
                    coatingCodeValue = codes[1].ToString();
                    

                }

                //pipeObject.MaterialType;
                var specObject = pipeObject.Spec;

               // specObject.GetPropertyValue();

           

                
            //    }

                if (structureObjectCount!=0)
                {
                 SupportingHelper supportingHelper = new SupportingHelper(oHgrSupport);
                  var structureObject = supportingHelper.SupportingObjectInfo(1);
                 width = structureObject.Width*1000;
                   
                 

                }
                switch (partNumber)
                {
                    
                    case "AS1":
                    case "AS2":
                        supportName = partNumber + "-" + nominalDiameter + "-" + coatingCodeValue + "-" + temperatureCodeValue;
                         break;
                    case "AS3":
                    case "AS4":
                      double repadLength=  Convert.ToDouble((oHgrSupport.GetPropertyValue("IJUAhs_RepadLength", "RepadLength")));
                      supportName = partNumber + "-" + nominalDiameter + "-" + repadLength + "-" + width + 2;
                       break;

                    case"AS6":
                        supportName = partNumber + "-" + nominalDiameter + "-" + temperatureCodeValue + "-" + coatingCodeValue;
                        break;
                    case "AS11":
                    case "AS12":
                    case "AS13":
                    case "AS14":
                        double shoeLength = Convert.ToDouble((oHgrSupport.GetPropertyValue("IJUAhs_ShoeLength", "ShoeLength")));
                        double shoeHeight = Convert.ToDouble((oHgrSupport.GetPropertyValue("IJUAhs_ShoeHeight", "ShoeHeight")));
                        supportName = partNumber + "-" + nominalDiameter + "-" + shoeHeight + "-" + width + 2 + "-" + shoeLength;
                        break;

                    case "AS16":
                    case "AS17":
                    case "AS18":
                        double stopShoeLength = Convert.ToDouble((oHgrSupport.GetPropertyValue("IJUAhs_ShoeLength", "ShoeLength")));
                        double stopShoeHeight = Convert.ToDouble((oHgrSupport.GetPropertyValue("IJUAhs_ShoeHeight", "ShoeHeight")));
                        supportName = partNumber + "-" + nominalDiameter + "-" + stopShoeHeight + "-" + stopShoeLength + width + 2 + "-" + temperatureCodeValue + coatingCodeValue;
                        break;
                    case "BP":
                    case "LUG":
                        supportName = partNumber;
                        break;
                    case "BPC1":
                        supportName = partNumber +"-"+ nominalDiameter;
                        break;

                    case "BPS1":
                        double wAttribute= Convert.ToDouble((oHgrSupport.GetPropertyValue("IJUAhs_ShoeLength", "ShoeLength"))); 
                        supportName = partNumber + "-" + nominalDiameter + wAttribute;
                        break;
                    case "BSC1":
                    case "BSC2":
                        string orientation = oHgrSupport.GetPropertyValue("IJUAhsSupportOrient", "SupportOrientation").ToString();
                        double length = Convert.ToDouble((oHgrSupport.GetPropertyValue("IJUAhs_ShoeLength", "ShoeLength")));
                        supportName = partNumber + "-" + nominalDiameter + length + orientation + temperatureCodeValue + coatingCodeValue;
                       break;
                    case "DS-1":
                        supportName = partNumber + "-" + nominalDiameter;
                        break;
                    case "FA11":
                        supportName = partNumber + "-" + nominalDiameter;
                        break;
                    case "GW13":
                        supportName = partNumber + "-" + nominalDiameter;
                        break;



                }
                var entityName = supportName;

                oEntity.SetPropertyValue(entityName, NAME_INTERFACE, NAME_PROPERTY);


            }

            catch (Exception ex)
            {
                string errorMessage = "Failed to compute Support name";
                errorMessage = ex.Message;

            }

        }

     

        public override Collection<BusinessObject> GetNamingParents(BusinessObject oEntity)
        {
            return new Collection<BusinessObject>();
        }


        
    }
}
