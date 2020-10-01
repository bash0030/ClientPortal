using System.Collections.Generic;
using System.Web.Mvc;
using ClientPortal.Models;
using ClientPortal.Helpers;
using Newtonsoft.Json;
using HIFIS.ENUMS.CommonClasses;
using System.Threading.Tasks;
using HIFIS.CONTRACTS.WCFContracts.DataContracts;
using System.ServiceModel;

namespace ClientPortal.Controllers
{
    public class ClientController : Controller
    {
        private SelectListItems selectListItems = new SelectListItems();
        private bool IsIllegalEntry(QuestionModel model)
        {
            return model.ClientInfo.FirstName == null;
        }

        private ActionResult HandlePostRequest(QuestionModel model)
        {
            TempData["Model"] = JsonConvert.SerializeObject(model);

            return RedirectToAction(model.NextQuestion, "Client");
        }

        private QuestionModel ResolveModel(QuestionModel model)
        {
            bool toSwitch = (model != null) ? true : false;
            model = model ?? new QuestionModel();

            if (TempData["Model"] != null)
            {
                QuestionModel qmodel = JsonConvert.DeserializeObject<QuestionModel>((string)TempData["Model"]);
                if (toSwitch)
                {
                    qmodel.FillEmpty(model);
                }
                model = qmodel;
                TempData.Remove("Model");
            }

            return model;
        }

        [HttpGet]
        public ActionResult FullName()
        {
            QuestionModel model = ResolveModel(null);

            model.IsLastQuestion = false;
            model.CurrentQuestion = "FullName";
            model.NextQuestion = "Aliases";
            model.PreviousQuestion = null;

            // prepare question
            model.Questions = new List<Question>() {
                new Question()
                {
                    QuestionText = "What is your name?",
                    HelpInfo = "Test",
                    UserInputDefinition = new UserInputDefinition()
                    {
                        Type = "multi-textbox",
                        Label = new string[] { "First Name", "Middle Name", "Last Name" },
                        Model = new string[] { "FirstName", "MiddleName", "LastName" },
                        Required = new bool[] { true, false, true },
                        Tooltip = new string[] { "The name you use on a daily basis", null, "Your family name" }
                    },
                }
            };

            return View(model);
        }

        [HttpPost]
        public ActionResult FullName(QuestionModel model)
        {
            model = ResolveModel(model);

            if (model.ClientInfo.FirstName == null || model.ClientInfo.LastName == null)
            {
                return View(model);
            }

            return HandlePostRequest(model);
        }

        [HttpGet]
        public ActionResult Aliases()
        {
            QuestionModel model = ResolveModel(null);

            if (IsIllegalEntry(model))
            {
                return RedirectToAction("Index", "Home");
            }

            model.IsLastQuestion = false;
            model.CurrentQuestion = "Aliases";
            model.NextQuestion = "CitizenshipImmigrationStatus";
            model.PreviousQuestion = "FullName";

            // prepare question
            model.Questions = new List<Question>() {
                new Question()
                {
                    QuestionText = "Have you used an alias?",
                    UserInputDefinition = new UserInputDefinition()
                    {
                        Type = "radio",
                        Model = new string[] { "HasAka" },
                        Options = selectListItems.YesNoBool
                    }
                },
                new Question()
                {
                    QuestionText = "What is your alias?",
                    UserInputDefinition = new UserInputDefinition()
                    {
                        Type = "multi-textbox",
                        Label = new string[] { "Alias 1", "Alias 2" },
                        Model = new string[] { "Aka1", "Aka2" },
                    }
                }
            };

            return View(model);
        }

        [HttpPost]
        public ActionResult Aliases(QuestionModel model)
        {
            model = ResolveModel(model);

            if (model.ClientInfo.HasAka && model.ClientInfo.Aka1 == null)
            {
                ModelState.AddModelError("Model.ClientInfo.Aka1", "Please enter the alias you have used");
                return View(model);
            }

            if (!model.ClientInfo.HasAka)
            {
                model.ClientInfo.Aka1 = null;
            }

            return HandlePostRequest(model);
        }


        [HttpGet]
        public ActionResult CitizenshipImmigrationStatus()
        {
            QuestionModel model = ResolveModel(null);

            if (IsIllegalEntry(model))
            {
                return RedirectToAction("Index", "Home");
            }

            model.PreviousQuestion = "Aliases";
            model.CurrentQuestion = "CitizenshipImmigrationStatus";
            model.NextQuestion = "DateOfBirth";
            model.IsLastQuestion = false;

            model.Questions = new List<Question>() {
                new Question()
                {
                    QuestionText = "What is your citizenship / immigration status?",
                    UserInputDefinition = new UserInputDefinition()
                    {
                        Type = "dropdown",
                        Label = new string[] { "Citizenship/Immigration Status" },
                        Model = new string[] { "CitizenshipTypeID" },
                        Options = selectListItems.CitizenshipTypeOptions,
                        Required = new bool[] { true }
                    }
                }
            };

            return View(model);
        }

        [HttpPost]
        public ActionResult CitizenshipImmigrationStatus(QuestionModel model)
        {
            model = ResolveModel(model);

            if (model.ClientInfo.CitizenshipTypeID <= 0)
            {
                return View(model);
            }

            return HandlePostRequest(model);
        }

        [HttpGet]
        public ActionResult DateOfBirth()
        {
            QuestionModel model = ResolveModel(null);

            if (IsIllegalEntry(model))
            {
                return RedirectToAction("Index", "Home");
            }

            model.IsLastQuestion = false;
            model.CurrentQuestion = "DateOfBirth";
            model.NextQuestion = "IndigenousStatus";
            model.PreviousQuestion = "CitizenshipImmigrationStatus";

            // prepare question
            model.Questions = new List<Question>() {
                new Question()
                {
                    QuestionText = "We would like to know your age group, would you like to enter your date of birth or an approximate age?",
                    HelpInfo = "Select approximate age if you do not wish to enter your actual date of birth.",
                    UserInputDefinition = new UserInputDefinition()
                    {
                        Type = "radio",
                        Model = new string[] { "UsedApproxAge" },
                        Options = selectListItems.UsedAproximateAgeOptions
                    }
                },
                new Question()
                {
                    QuestionText = "Please enter your date of birth.",
                    UserInputDefinition = new UserInputDefinition()
                    {
                        Type = "date",
                        Label = new string[] { "Date of Birth" },
                        Model = new string[] { "DOB" },
                        Required = new bool[] { true }
                    },
                },
                new Question()
                {
                    QuestionText = "Please enter an approximate age.",
                    UserInputDefinition = new UserInputDefinition()
                    {
                        Type = "textbox",
                        InputType = new string[] { "number" },
                        Label = new string[] { "Approximate Age" },
                        Model = new string[] { "AproximativeAge" },
                        Required = new bool[] { true }
                    },
                } 
            };

            return View(model);
        }

        [HttpPost]
        public ActionResult DateOfBirth(QuestionModel model)
        {
            model = ResolveModel(model);

            bool noErrors = (model.ClientInfo.UsedApproxAge && model.ClientInfo.AproximativeAge > 0 && model.ClientInfo.AproximativeAge <= 100) ||
                                (!model.ClientInfo.UsedApproxAge && model.ClientInfo.DOB.Year > 1930);

            if (!noErrors)
            {
                return View(model);
            }

            return HandlePostRequest(model);
        }

        [HttpGet]
        public ActionResult IndigenousStatus()
        {
            QuestionModel model = ResolveModel(null);

            if (IsIllegalEntry(model))
            {
                return RedirectToAction("Index", "Home");
            }

            model.IsLastQuestion = false;
            model.CurrentQuestion = "IndigenousStatus";
            model.NextQuestion = "Gender";
            model.PreviousQuestion = "CitizenshipImmigrationStatus";

            // prepare question
            var opt = selectListItems.IndigenousStatusOptions;
            opt.Remove(opt.Find(o => "Non Aboriginal".Equals(o.Text)));

            model.Questions = new List<Question>() {
                new Question()
                {
                    QuestionText = "Are you a person of Indigenous descent?",
                    UserInputDefinition = new UserInputDefinition()
                    {
                        Type = "radio",
                        Model = new string[] { "IsIndigenous" },
                        Options = selectListItems.YesNoBool
                    }
                },
                new Question()
                {
                    QuestionText = "Please choose the category that best describes you.",
                    UserInputDefinition = new UserInputDefinition()
                    {
                        Type = "dropdown",
                        Label = new string[] { "Indigenous Status" },
                        Model = new string[] { "AboriginalIndicatorID" },
                        Required = new bool[] { true },
                        Options = opt
                    }
                }
            };

            return View(model);
        }

        [HttpPost]
        public ActionResult IndigenousStatus(QuestionModel model)
        {
            model = ResolveModel(model);

            if (model.ClientInfo.IsIndigenous && model.ClientInfo.AboriginalIndicatorID <= 0)
            {
                return View(model);
            }

            if (!model.ClientInfo.IsIndigenous)
            {
                model.ClientInfo.AboriginalIndicatorID = (short)IndianStatusTypes.NonAboriginal;
            }

            return HandlePostRequest(model);
        }

        [HttpGet]
        public ActionResult Gender()
        {
            QuestionModel model = ResolveModel(null);

            if (IsIllegalEntry(model))
            {
                return RedirectToAction("Index", "Home");
            }

            model.PreviousQuestion = "IndigenousStatus";
            model.CurrentQuestion = "Gender";
            model.NextQuestion = "MedicAlert";
            model.IsLastQuestion = false;

            model.Questions = new List<Question>() {
                new Question()
                {
                    QuestionText = "What is your gender? Select an option that best describes you.",
                    UserInputDefinition = new UserInputDefinition()
                    {
                        Type = "dropdown",
                        Label = new string[] { "Gender" },
                        Model = new string[] { "Gender" },
                        Options = selectListItems.GenderOptions,
                        Required = new bool[] { true }
                    }
                }
            };

            return View(model);
        } 

        [HttpPost]
        public ActionResult Gender(QuestionModel model)
        {
            model = ResolveModel(model);

            if (model.ClientInfo.Gender <= 0)
            {
                return View(model);
            }

            return HandlePostRequest(model);
        }

        [HttpGet]
        public ActionResult MedicAlert()
        {
            QuestionModel model = ResolveModel(null);

            if (IsIllegalEntry(model))
            {
                return RedirectToAction("Index", "Home");
            }

            model.PreviousQuestion = "Gender";
            model.CurrentQuestion = "MedicAlert";
            model.NextQuestion = "Disability";
            model.IsLastQuestion = false;

            model.Questions = new List<Question>() {
                new Question()
                {
                    QuestionText = "Do you have a medic alert?",
                    UserInputDefinition = new UserInputDefinition()
                    {
                        Type = "radio",
                        Model = new string[] { "HasMedicAlert" },
                        Options = selectListItems.YesNoBool,
                    }
                }
            };

            return View(model);
        } 

        [HttpPost]
        public ActionResult MedicAlert(QuestionModel model)
        {
            model = ResolveModel(model);

            model.ClientInfo.MedicAlertYN = (model.ClientInfo.HasMedicAlert) ? "yes" : "no";

            return HandlePostRequest(model);
        }

        [HttpGet]
        public ActionResult Disability()
        {
            QuestionModel model = ResolveModel(null);

            if (IsIllegalEntry(model))
            {
                return RedirectToAction("Index", "Home");
            }

            model.PreviousQuestion = "MedicAlert";
            model.CurrentQuestion = "Disability";
            model.NextQuestion = "VeteranStatus";
            model.IsLastQuestion = false;

            model.Questions = new List<Question>() {
                new Question()
                {
                    QuestionText = "Do you have any disabilities?",
                    UserInputDefinition = new UserInputDefinition()
                    {
                        Type = "radio",
                        Model = new string[] { "HasDisability" },
                        Options = selectListItems.YesNoBool,
                    }
                }
            };

            return View(model);
        } 

        [HttpPost]
        public ActionResult Disability(QuestionModel model)
        {
            model = ResolveModel(model);

            model.ClientInfo.DisabilityYN = (model.ClientInfo.HasDisability) ? "yes" : "no";

            return HandlePostRequest(model);
        }

        [HttpGet]
        public ActionResult VeteranStatus()
        {
            QuestionModel model = ResolveModel(null);

            if (IsIllegalEntry(model))
            {
                return RedirectToAction("Index", "Home");
            }

            model.IsLastQuestion = false;
            model.CurrentQuestion = "VeteranStatus";
            model.NextQuestion = "CountryOfBirth";
            model.PreviousQuestion = "Disability";

            // prepare question
            var opt = selectListItems.VeteranStatusOptions;
            opt.Remove(opt.Find(o => "Non Veteran".Equals(o.Text)));

            model.Questions = new List<Question>() {
                new Question()
                {
                    QuestionText = "Are you a veteran?",
                    UserInputDefinition = new UserInputDefinition()
                    {
                        Type = "radio",
                        Model = new string[] { "IsVeteran" },
                        Options = selectListItems.YesNoBool
                    }
                },
                new Question()
                {
                    QuestionText = "Please choose the category that best describes you.",
                    UserInputDefinition = new UserInputDefinition()
                    {
                        Type = "dropdown",
                        Label = new string[] { "Veteran Status" },
                        Model = new string[] { "VeteranStateID" },
                        Required = new bool[] { true },
                        Options = opt
                    }
                }
            };

            return View(model);
        }

        [HttpPost]
        public ActionResult VeteranStatus(QuestionModel model)
        {
            model = ResolveModel(model);

            if (model.ClientInfo.IsVeteran && model.ClientInfo.VeteranStateID <= 0)
            {
                return View(model);
            }

            if (!model.ClientInfo.IsVeteran)
            {
                model.ClientInfo.VeteranStateID = (short)VenteranStateTypes.NonVeteran;
            }

            return HandlePostRequest(model);
        }

        [HttpGet]
        public ActionResult CountryOfBirth()
        {
            QuestionModel model = ResolveModel(null);

            if (IsIllegalEntry(model))
            {
                return RedirectToAction("Index", "Home");
            }

            model.PreviousQuestion = "VeteranStatus";
            model.CurrentQuestion = "CountryOfBirth";
            model.NextQuestion = "Review";
            model.IsLastQuestion = false;

            model.Questions = new List<Question>() {
                new Question()
                {
                    QuestionText = "Where were you born?",
                    UserInputDefinition = new UserInputDefinition()
                    {
                        Type = "dropdown",
                        Label = new string[] { "Country of Birth" },
                        Model = new string[] { "CountryOfBirth" },
                        Options = selectListItems.CountryOptions,
                        Required = new bool[] { false }
                    }
                }
            };

            return View(model);
        }

        [HttpPost]
        public ActionResult CountryOfBirth(QuestionModel model)
        {
            model = ResolveModel(model);

            if (model.ClientInfo.CountryOfBirth <= 0)
            {
                return View(model);
            }

            return HandlePostRequest(model);
        }

        public ActionResult Review()
        {
            QuestionModel model = ResolveModel(null);

            if (IsIllegalEntry(model))
            {
                return RedirectToAction("Index", "Home");
            }

            return View(model.ClientInfo);
        }

        public async Task<ActionResult> Submit(string data)
        {
            ClientInfo info = JsonConvert.DeserializeObject<ClientInfo>(data);

            ClientVitals cv = new ClientVitals()
            {
                FirstName = info.FirstName,
                MiddleName = info.MiddleName,
                LastName = info.LastName,
                GenderTypeID = info.Gender,
                Aka1 = info.Aka1,
                Aka2 = info.Aka2,
                CountryOfBirth = info.CountryOfBirth,
                DOB = info.DOB,
                AproximativeAge = info.AproximativeAge,
                DisabilityYN = info.DisabilityYN,
                MedicAlertYN = info.MedicAlertYN,
                VeteranStateID = info.VeteranStateID,
                CitizenshipTypeID = info.CitizenshipTypeID,
                AboriginalIndicatorID = info.AboriginalIndicatorID,
                IsStealthYN = "Y"
            };

            ViewBag.Success = true;

            var authService = new AuthenticationService.AuthenticationServiceClient();

            using (var scope = authService.InnerChannel)
            {
                var token = await authService.ValidateUserAsync("admin", "123456", 1, "", Request.RequestContext.HttpContext.Session.SessionID);

                try
                {
                    var clientService = new ClientService.ClientServiceClient();
                    WCFValidationResult result = clientService.InsertVitals(cv, token);

                    ViewBag.Success = result.Success;

                    if (!result.Success)
                    {
                        logless(result.ValidationMessages[0], "Result message");
                    }
                }
                catch (FaultException e)
                {
                    logless(e.Message, "Exception message");

                    ViewBag.Success = false;
                }
            }

            return View("Confirm");
        }

        public void logless(object obj, string label)
        {
            System.Diagnostics.Debug.WriteLine("-----------------------------");
            System.Diagnostics.Debug.WriteLine(label.Trim() + " => " + obj);
            System.Diagnostics.Debug.WriteLine("-----------------------------");
        }
    }
}