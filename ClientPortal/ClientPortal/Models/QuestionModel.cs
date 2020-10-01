using System.Collections.Generic;
using System;
using System.Reflection;

namespace ClientPortal.Models
{
    public class QuestionModel
    {
        public IEnumerable<Question> Questions { set; get; }
        public ClientInfo ClientInfo { set; get; }
        public string NextQuestion { set; get; }
        public string CurrentQuestion { set; get; }
        public string PreviousQuestion { set; get; }
        public bool IsLastQuestion { set; get; }

        public QuestionModel()
        {
            IsLastQuestion = false;
             ClientInfo = new ClientInfo();
            Questions = new List<Question>();
        }

        public void FillEmpty(QuestionModel patch)
        {
            if (ClientInfo != null)
            {
                foreach (PropertyInfo prop in ClientInfo.GetType().GetProperties())
                {
                    var thisVal = prop.GetValue(ClientInfo, null);
                    var patchVal = prop.GetValue(patch.ClientInfo, null);

                    if (thisVal != null)
                    {
                        switch (thisVal.GetType().Name)
                        {
                            case "Int16":

                                if ((short)thisVal <= 0)
                                {
                                    if ((short)patchVal > 0)
                                    {
                                        prop.SetValue(ClientInfo, patchVal);
                                    }
                                }

                                break;

                            case "Boolean":

                                prop.SetValue(ClientInfo, patchVal);

                                break;

                            case "DateTime":

                                DateTime dt = new DateTime();
                                DateTime val = (DateTime)patchVal;

                                bool initVal = dt.Day.Equals(val.Day) && dt.Month.Equals(val.Month) && dt.Year.Equals(val.Year);

                                if (!initVal)
                                {
                                    prop.SetValue(ClientInfo, patchVal);
                                }

                                break;

                            default:

                                if (patchVal != null)
                                {
                                    prop.SetValue(ClientInfo, patchVal);
                                }
                                
                                break;
                        }
                    }

                    else
                    {
                        if (patchVal != null)
                        {
                            prop.SetValue(ClientInfo, patchVal);
                        }
                    }
                }
            }
        }
    }
}