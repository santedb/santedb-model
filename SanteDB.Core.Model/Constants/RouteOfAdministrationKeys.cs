/*
 * Copyright (C) 2021 - 2024, SanteSuite Inc. and the SanteSuite Contributors (See NOTICE.md for full copyright notices)
 * Copyright (C) 2019 - 2021, Fyfe Software Inc. and the SanteSuite Contributors
 * Portions Copyright (C) 2015-2018 Mohawk College of Applied Arts and Technology
 * 
 * Licensed under the Apache License, Version 2.0 (the "License"); you 
 * may not use this file except in compliance with the License. You may 
 * obtain a copy of the License at 
 * 
 * http://www.apache.org/licenses/LICENSE-2.0 
 * 
 * Unless required by applicable law or agreed to in writing, software
 * distributed under the License is distributed on an "AS IS" BASIS, WITHOUT
 * WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied. See the 
 * License for the specific language governing permissions and limitations under 
 * the License.
 * 
 * User: fyfej
 * Date: 2023-6-21
 */
using System;

namespace SanteDB.Core.Model.Constants
{
    /// <summary>
    /// Default routes of administration
    /// </summary>
    public static class RouteOfAdministrationKeys
    {
        /// <summary> Injection, amniotic fluid                                                                                                                                                                                                                                                                                                                                                   
        ///</summary>
        public static readonly Guid InjectionAmnioticFluid = Guid.Parse("0CA599E6-8B4A-4122-B2B9-6085A14EFB5B");
        /// <summary> Injection, biliary Tract                                                                                                                                                                                                                                                                                                                                                     
        ///</summary>
        public static readonly Guid InjectionBiliaryTract = Guid.Parse("3B139D49-42D8-4DCA-8121-FF45509FABEB");
        /// <summary> Injection, urinary bladder                                                                                                                                                                                                                                                                                                                                                 
        ///</summary>
        public static readonly Guid InjectionUrinaryBladder = Guid.Parse("4700E7ED-2DEA-4978-9135-575E20B85BA1");
        /// <summary> Instillation, urinary Catheter                                                                                                                                                                                                                                                                                                                                         
        ///</summary>
        public static readonly Guid InstillationUrinaryCatheter = Guid.Parse("E4B78CFB-474C-4892-92EB-9365D63F407C");
        /// <summary> Irrigation, urinary bladder                                                                                                                                                                                                                                                                                                                                               
        ///</summary>
        public static readonly Guid IrrigationUrinaryBladder = Guid.Parse("8F0B8300-796D-42C1-99DB-C18811AB9744");
        /// <summary> Irrigation, urinary bladder, Continuous                                                                                                                                                                                                                                                                                                                        
        ///</summary>
        public static readonly Guid IrrigationUrinaryBladderContinuous = Guid.Parse("E362372E-B93B-4E99-9AB8-A365A417A8B4");
        /// <summary> Irrigation, urinary bladder, Tidal                                                                                                                                                                                                                                                                                                                                  
        ///</summary>
        public static readonly Guid IrrigationUrinaryBladderTidal = Guid.Parse("79746BE7-44FE-4FEA-B0CF-DD800E940C02");
        /// <summary> Topical application, buccal                                                                                                                                                                                                                                                                                                                                               
        ///</summary>
        public static readonly Guid TopicalApplicationBuccal = Guid.Parse("66EA56B7-92AE-4DEC-B99F-9260ABB192F3");
        /// <summary> Instillation, Continuous ambulatory peritoneal dialysis port                                                                                                                                                                                                                                                                             
        ///</summary>
        public static readonly Guid InstillationContinuousAmbulatoryPeritonealDialysisPort = Guid.Parse("CAB854E3-9643-4223-9623-1C6DB77054ED");
        /// <summary> Instillation, Cecostomy                                                                                                                                                                                                                                                                                                                                                       
        ///</summary>
        public static readonly Guid InstillationCecostomy = Guid.Parse("038F8CDE-5ED3-4EB9-8241-B8DF07E7B691");
        /// <summary> Topical application, Cervical                                                                                                                                                                                                                                                                                                                                           
        ///</summary>
        public static readonly Guid TopicalApplicationCervical = Guid.Parse("B58599E5-8A88-451B-8CE3-496206BD7D36");
        /// <summary> Injection, Cervical                                                                                                                                                                                                                                                                                                                                                               
        ///</summary>
        public static readonly Guid InjectionCervical = Guid.Parse("6398D262-23E6-4929-AEAA-777677FD6D3B");
        /// <summary> Insertion, Cervical (uterine)                                                                                                                                                                                                                                                                                                                                           
        ///</summary>
        public static readonly Guid InsertionCervicalUterine = Guid.Parse("EC64549C-6A06-447E-A7D9-B262518FC885");
        /// <summary> Chew, oral                                                                                                                                                                                                                                                                                                                                                                                 
        ///</summary>
        public static readonly Guid ChewOral = Guid.Parse("752CF2CD-F592-40C0-A867-CE55BE98339C");
        /// <summary> Injection, for Cholangiography                                                                                                                                                                                                                                                                                                                                         
        ///</summary>
        public static readonly Guid InjectionForCholangiography = Guid.Parse("BBEF69C0-0B60-4040-9D3D-6341245D7FC0");
        /// <summary> Instillation, Chest Tube                                                                                                                                                                                                                                                                                                                                                     
        ///</summary>
        public static readonly Guid InstillationChestTube = Guid.Parse("478C2CCB-6F56-475F-BCEC-B14B679F51D8");
        /// <summary> Topical application, dental                                                                                                                                                                                                                                                                                                                                               
        ///</summary>
        public static readonly Guid TopicalApplicationDental = Guid.Parse("8EAC881C-A055-4C64-9E60-213A4658B14A");
        /// <summary> rinse, dental                                                                                                                                                                                                                                                                                                                                                                           
        ///</summary>
        public static readonly Guid RinseDental = Guid.Parse("1E253DBB-EBEF-4327-ACE9-1F1D6D10C3B3");
        /// <summary> dissolve, oral                                                                                                                                                                                                                                                                                                                                                                         
        ///</summary>
        public static readonly Guid DissolveOral = Guid.Parse("B0AC15BE-8474-44D7-BC65-CA0D0DFC8FCA");
        /// <summary> douche, vaginal                                                                                                                                                                                                                                                                                                                                                                       
        ///</summary>
        public static readonly Guid DoucheVaginal = Guid.Parse("8AD8D4E2-3185-458B-AD14-24FDECB4430C");
        /// <summary> Topical application, soaked Dressing                                                                                                                                                                                                                                                                                                                             
        ///</summary>
        public static readonly Guid TopicalApplicationSoakedDressing = Guid.Parse("B7B2076C-3A2F-4503-AD82-1E98D7D41507");
        /// <summary> Instillation, enteral feeding Tube                                                                                                                                                                                                                                                                                                                                 
        ///</summary>
        public static readonly Guid InstillationEnteralFeedingTube = Guid.Parse("833DC098-0847-4C3F-ADA5-C7F2AAF34104");
        /// <summary> electro-osmosis                                                                                                                                                                                                                                                                                                                                                                      
        ///</summary>
        public static readonly Guid ElectroOsmosis = Guid.Parse("F46DFFF1-2E37-4E7A-914E-C65EDF3FE809");
        /// <summary> Injection, endosinusial                                                                                                                                                                                                                                                                                                                                                       
        ///</summary>
        public static readonly Guid InjectionEndosinusial = Guid.Parse("A40541AE-28D7-40AE-9041-1CAE7D5FAD95");
        /// <summary> enema, rectal                                                                                                                                                                                                                                                                                                                                                                           
        ///</summary>
        public static readonly Guid EnemaRectal = Guid.Parse("BB7CEE09-4470-4A59-9EBB-E63270AE970F");
        /// <summary> Instillation, enteral                                                                                                                                                                                                                                                                                                                                                           
        ///</summary>
        public static readonly Guid InstillationEnteral = Guid.Parse("EB09B294-1603-41E3-8946-BF11ABAADD3F");
        /// <summary> Infusion, epidural                                                                                                                                                                                                                                                                                                                                                                 
        ///</summary>
        public static readonly Guid InfusionEpidural = Guid.Parse("B21023A1-C5C1-4D3B-98AC-ED2E05C5C248");
        /// <summary> Injection, epidural                                                                                                                                                                                                                                                                                                                                                               
        ///</summary>
        public static readonly Guid InjectionEpidural = Guid.Parse("36A06DE6-5519-49A7-8511-70583A841457");
        /// <summary> Injection, epidural, push                                                                                                                                                                                                                                                                                                                                                    
        ///</summary>
        public static readonly Guid InjectionEpiduralPush = Guid.Parse("538DA96F-CAD5-4501-AFA4-8CBD32553DF3");
        /// <summary> Injection, epidural, slow push                                                                                                                                                                                                                                                                                                                                          
        ///</summary>
        public static readonly Guid InjectionEpiduralSlowPush = Guid.Parse("A30972D5-E9B9-4A37-9257-77B6324C1413");
        /// <summary> Instillation, endotracheal Tube                                                                                                                                                                                                                                                                                                                                       
        ///</summary>
        public static readonly Guid InstillationEndotrachealTube = Guid.Parse("0CE7CD7C-5971-4F19-887B-1A58398D1EF5");
        /// <summary> nebulization, endotracheal Tube                                                                                                                                                                                                                                                                                                                                       
        ///</summary>
        public static readonly Guid nebulizationEndotrachealTube = Guid.Parse("3E977A4C-7CC1-4DBF-B0CE-536751E2A479");
        /// <summary> diffusion, extracorporeal                                                                                                                                                                                                                                                                                                                                                   
        ///</summary>
        public static readonly Guid DiffusionExtracorporeal = Guid.Parse("A002E89D-92C6-44CD-AB86-B1DA52365EA6");
        /// <summary> Injection, extracorporeal                                                                                                                                                                                                                                                                                                                                                   
        ///</summary>
        public static readonly Guid InjectionExtracorporeal = Guid.Parse("849DAF7C-CD07-448D-9F1C-0D53452A8826");
        /// <summary> Injection, extra-amniotic                                                                                                                                                                                                                                                                                                                                                   
        ///</summary>
        public static readonly Guid InjectionExtraAmniotic = Guid.Parse("630CC4AC-53CB-47CE-910C-D371FA1065D8");
        /// <summary> gargle                                                                                                                                                                                                                                                                                                                                                                                        
        ///</summary>
        public static readonly Guid Gargle = Guid.Parse("FBA1B28D-2255-4419-A6CA-D52A60608A63");
        /// <summary> Injection, gastric button                                                                                                                                                                                                                                                                                                                                                   
        ///</summary>
        public static readonly Guid InjectionGastricButton = Guid.Parse("53D5D3CC-5ECC-42E4-9482-EC26CE8E61DC");
        /// <summary> Topical application, gingival                                                                                                                                                                                                                                                                                                                                           
        ///</summary>
        public static readonly Guid TopicalApplicationGingival = Guid.Parse("7E26EC93-CC57-43DC-BDAA-F0FDE3EC3309");
        /// <summary> Injection, gingival                                                                                                                                                                                                                                                                                                                                                               
        ///</summary>
        public static readonly Guid InjectionGingival = Guid.Parse("CC072F1C-D326-4B49-BFE6-2FEF2B25C36F");
        /// <summary> Instillation, gastro-jejunostomy Tube                                                                                                                                                                                                                                                                                                                           
        ///</summary>
        public static readonly Guid InstillationGastroJejunostomyTube = Guid.Parse("B711A692-4726-4B3C-B411-9847C76A172E");
        /// <summary> Instillation, gastrostomy Tube                                                                                                                                                                                                                                                                                                                                         
        ///</summary>
        public static readonly Guid InstillationGastrostomyTube = Guid.Parse("19A55C96-6B50-4EA6-AAAD-B5109A591E30");
        /// <summary> Irrigation, genitourinary                                                                                                                                                                                                                                                                                                                                                   
        ///</summary>
        public static readonly Guid IrrigationGenitourinary = Guid.Parse("0D0E314E-0CE7-4A7A-B300-95AA76AF35A5");
        /// <summary> Topical application, hair                                                                                                                                                                                                                                                                                                                                                   
        ///</summary>
        public static readonly Guid TopicalApplicationHair = Guid.Parse("38C5173B-4A6B-4C4A-B437-244F598F9725");
        /// <summary> diffusion, hemodialysis                                                                                                                                                                                                                                                                                                                                                       
        ///</summary>
        public static readonly Guid DiffusionHemodialysis = Guid.Parse("009047E8-81F0-4E25-BB22-D2B74F685C2D");
        /// <summary> Injection, hemodialysis port                                                                                                                                                                                                                                                                                                                                             
        ///</summary>
        public static readonly Guid InjectionHemodialysisPort = Guid.Parse("B9736FE9-75B6-46E0-A816-88602512EAE8");
        /// <summary> Infusion, Intraarterial Catheter                                                                                                                                                                                                                                                                                                                                     
        ///</summary>
        public static readonly Guid InfusionIntraarterialCatheter = Guid.Parse("77ED18F1-E59C-4CB7-8845-A9E3EB866483");
        /// <summary> Injection, Intra-abdominal                                                                                                                                                                                                                                                                                                                                                 
        ///</summary>
        public static readonly Guid InjectionIntraAbdominal = Guid.Parse("354388C5-A10B-4D9C-9E62-254A9CE105F1");
        /// <summary> Injection, Intraarterial                                                                                                                                                                                                                                                                                                                                                     
        ///</summary>
        public static readonly Guid InjectionIntraarterial = Guid.Parse("54D81CDC-1260-4433-A665-805DEB1BABB3");
        /// <summary> Injection, Intraarterial, push                                                                                                                                                                                                                                                                                                                                          
        ///</summary>
        public static readonly Guid InjectionIntraarterialPush = Guid.Parse("30574800-7E80-400E-A90B-43DF603CC531");
        /// <summary> Injection, Intraarterial, slow push                                                                                                                                                                                                                                                                                                                                
        ///</summary>
        public static readonly Guid InjectionIntraarterialSlowPush = Guid.Parse("D21D3E62-A5CE-4D04-A24A-0F75825313DE");
        /// <summary> Injection, Intraarticular                                                                                                                                                                                                                                                                                                                                                   
        ///</summary>
        public static readonly Guid InjectionIntraarticular = Guid.Parse("22142D87-FA96-4EF1-9FE2-42DC84B8E044");
        /// <summary> Instillation, Intrabronchial                                                                                                                                                                                                                                                                                                                                             
        ///</summary>
        public static readonly Guid InstillationIntrabronchial = Guid.Parse("D43792AF-7D5F-4856-BA0B-A8B6B2B639F2");
        /// <summary> Injection, Intrabursal                                                                                                                                                                                                                                                                                                                                                         
        ///</summary>
        public static readonly Guid InjectionIntrabursal = Guid.Parse("52D1509B-DE13-48E3-9312-7CF6835992ED");
        /// <summary> Infusion, Intracardiac                                                                                                                                                                                                                                                                                                                                                         
        ///</summary>
        public static readonly Guid InfusionIntracardiac = Guid.Parse("703C55BF-EBBC-43B9-84C8-FC32BECCC06C");
        /// <summary> Injection, Intracardiac                                                                                                                                                                                                                                                                                                                                                       
        ///</summary>
        public static readonly Guid InjectionIntracardiac = Guid.Parse("AADE10A7-F6DE-456A-B16D-66C489682B93");
        /// <summary> Injection, Intracardiac, rapid push                                                                                                                                                                                                                                                                                                                                
        ///</summary>
        public static readonly Guid InjectionIntracardiacRapidPush = Guid.Parse("D7C02834-9E37-4806-A963-5DDC421F70FA");
        /// <summary> Injection, Intracardiac, slow push                                                                                                                                                                                                                                                                                                                                  
        ///</summary>
        public static readonly Guid InjectionIntracardiacSlowPush = Guid.Parse("069AEBE4-2159-4223-B01B-11BB45244A24");
        /// <summary> Injection, Intracardiac, push                                                                                                                                                                                                                                                                                                                                            
        ///</summary>
        public static readonly Guid InjectionIntracardiacPush = Guid.Parse("A7DDD439-AE2D-4B41-8640-3AFC0B056D11");
        /// <summary> Injection, Intracartilaginous                                                                                                                                                                                                                                                                                                                                           
        ///</summary>
        public static readonly Guid InjectionIntracartilaginous = Guid.Parse("1FA3A51F-AA5F-419E-B6D2-677EFAD0BBFF");
        /// <summary> Injection, Intracaudal                                                                                                                                                                                                                                                                                                                                                         
        ///</summary>
        public static readonly Guid InjectionIntracaudal = Guid.Parse("C8B9A6F3-7A57-4A0D-9D10-BCEB1436239E");
        /// <summary> Injection, Intracavernous                                                                                                                                                                                                                                                                                                                                                   
        ///</summary>
        public static readonly Guid InjectionIntracavernous = Guid.Parse("31CD7748-C762-458F-B528-9CBF94C8A006");
        /// <summary> Injection, Intracavitary                                                                                                                                                                                                                                                                                                                                                     
        ///</summary>
        public static readonly Guid InjectionIntracavitary = Guid.Parse("5BEAE77D-BA49-4E8E-A938-E8C9466FB906");
        /// <summary> Injection, Intracerebral                                                                                                                                                                                                                                                                                                                                                     
        ///</summary>
        public static readonly Guid InjectionIntracerebral = Guid.Parse("0B48F99D-8E5B-468C-B13E-D0EF5F0E0ABB");
        /// <summary> Injection, Intracisternal                                                                                                                                                                                                                                                                                                                                                   
        ///</summary>
        public static readonly Guid InjectionIntracisternal = Guid.Parse("EBCD3697-45BE-469D-AC7A-EF67B725869F");
        /// <summary> Infusion, Intracoronary                                                                                                                                                                                                                                                                                                                                                       
        ///</summary>
        public static readonly Guid InfusionIntracoronary = Guid.Parse("EEAEE138-BF49-4E73-B000-5BD506BAE600");
        /// <summary> Topical application, Intracorneal                                                                                                                                                                                                                                                                                                                                   
        ///</summary>
        public static readonly Guid TopicalApplicationIntracorneal = Guid.Parse("679C378C-2FF9-4275-8117-313427E9111D");
        /// <summary> Injection, Intracoronary                                                                                                                                                                                                                                                                                                                                                     
        ///</summary>
        public static readonly Guid InjectionIntracoronary = Guid.Parse("43AAC5BB-C2F7-45CB-B433-2FD0350C07A1");
        /// <summary> Injection, Intracoronary, push                                                                                                                                                                                                                                                                                                                                          
        ///</summary>
        public static readonly Guid InjectionIntracoronaryPush = Guid.Parse("61B871D3-BD44-432A-80FA-F325CD17AC4A");
        /// <summary> Topical application, Intracoronal (dental)                                                                                                                                                                                                                                                                                                                 
        ///</summary>
        public static readonly Guid TopicalApplicationIntracoronalDental = Guid.Parse("BC120C42-365C-40EF-A07D-60AA4A6F5E72");
        /// <summary> Injection, Intracorpus Cavernosum                                                                                                                                                                                                                                                                                                                                   
        ///</summary>
        public static readonly Guid InjectionIntracorpusCavernosum = Guid.Parse("1AFAD697-3102-4B9A-870D-3700A845CCD2");
        /// <summary> Implantation, Intradermal                                                                                                                                                                                                                                                                                                                                                   
        ///</summary>
        public static readonly Guid ImplantationIntradermal = Guid.Parse("E56B859A-7039-4E0C-98E0-F95FAADB9507");
        /// <summary> Injection, Intradermal                                                                                                                                                                                                                                                                                                                                                         
        ///</summary>
        public static readonly Guid InjectionIntradermal = Guid.Parse("AC31BF4F-CD14-46D4-AF57-ECB21C221B88");
        /// <summary> Injection, Intradiscal                                                                                                                                                                                                                                                                                                                                                         
        ///</summary>
        public static readonly Guid InjectionIntradiscal = Guid.Parse("31850C1F-BE15-459A-A17D-E405A1FE7842");
        /// <summary> mucosal absorption, Intraduodenal                                                                                                                                                                                                                                                                                                                                   
        ///</summary>
        public static readonly Guid MucosalAbsorptionIntraduodenal = Guid.Parse("6544A572-CE8B-4F8E-ABA5-FFA1FAA6D4CA");
        /// <summary> Injection, Intraductal                                                                                                                                                                                                                                                                                                                                                         
        ///</summary>
        public static readonly Guid InjectionIntraductal = Guid.Parse("1B51F90A-56EB-4902-8460-4362E908AFEB");
        /// <summary> Instillation, Intraduodenal                                                                                                                                                                                                                                                                                                                                               
        ///</summary>
        public static readonly Guid InstillationIntraduodenal = Guid.Parse("2F98F4A3-E2A8-45AF-AE71-725787DE7A40");
        /// <summary> Injection, Intradural                                                                                                                                                                                                                                                                                                                                                           
        ///</summary>
        public static readonly Guid InjectionIntradural = Guid.Parse("E5C6F2B8-624A-4E69-94E9-423F5EB9E50E");
        /// <summary> Injection, Intraepidermal                                                                                                                                                                                                                                                                                                                                                   
        ///</summary>
        public static readonly Guid InjectionIntraepidermal = Guid.Parse("3FE6623F-BEBB-4246-953F-93C441587D3C");
        /// <summary> Injection, Intraepithelial                                                                                                                                                                                                                                                                                                                                                 
        ///</summary>
        public static readonly Guid InjectionIntraepithelial = Guid.Parse("BC9DC8BE-8B11-4E3E-B47D-6DB069BACE4A");
        /// <summary> Instillation, Intraesophageal                                                                                                                                                                                                                                                                                                                                           
        ///</summary>
        public static readonly Guid InstillationIntraesophageal = Guid.Parse("405F7CDF-363D-4D8C-82D3-33FC695096C1");
        /// <summary> Topical application, Intraesophageal                                                                                                                                                                                                                                                                                                                             
        ///</summary>
        public static readonly Guid TopicalApplicationIntraesophageal = Guid.Parse("AA4127CB-72C0-4B6F-A557-73FAC4BD92FA");
        /// <summary> Instillation, Intragastric                                                                                                                                                                                                                                                                                                                                                 
        ///</summary>
        public static readonly Guid InstillationIntragastric = Guid.Parse("06FFA8A0-D04E-4487-9CCB-3FF217384E35");
        /// <summary> Irrigation, Intragastric                                                                                                                                                                                                                                                                                                                                                     
        ///</summary>
        public static readonly Guid IrrigationIntragastric = Guid.Parse("6F983710-6084-45C6-B9B3-A0C30000FA14");
        /// <summary> lavage, Intragastric                                                                                                                                                                                                                                                                                                                                                             
        ///</summary>
        public static readonly Guid lavageIntragastric = Guid.Parse("9DFF5EF1-2D4D-423B-97CD-D8D4384C215E");
        /// <summary> Instillation, Intraileal                                                                                                                                                                                                                                                                                                                                                     
        ///</summary>
        public static readonly Guid InstillationIntraileal = Guid.Parse("241C8E93-5601-4EDE-8033-81A32BFABA57");
        /// <summary> Topical application, Intraileal                                                                                                                                                                                                                                                                                                                                       
        ///</summary>
        public static readonly Guid TopicalApplicationIntraileal = Guid.Parse("2E83FAD2-EE03-4269-9FE0-E78F503EAA45");
        /// <summary> Injection, Intralesional                                                                                                                                                                                                                                                                                                                                                     
        ///</summary>
        public static readonly Guid InjectionIntralesional = Guid.Parse("173021C8-23C6-49FF-A770-4D30F8F03D20");
        /// <summary> Irrigation, Intralesional                                                                                                                                                                                                                                                                                                                                                   
        ///</summary>
        public static readonly Guid IrrigationIntralesional = Guid.Parse("24F2D8C8-8865-4C39-A8C6-38C0BB835336");
        /// <summary> Topical application, Intralesional                                                                                                                                                                                                                                                                                                                                 
        ///</summary>
        public static readonly Guid TopicalApplicationIntralesional = Guid.Parse("3D6E3F67-F0A7-47D9-A8B8-F03B0EB0DF79");
        /// <summary> Injection, Intraluminal                                                                                                                                                                                                                                                                                                                                                       
        ///</summary>
        public static readonly Guid InjectionIntraluminal = Guid.Parse("9D4E149E-B50E-4151-BB2C-5CD80E13772A");
        /// <summary> Topical application, Intraluminal                                                                                                                                                                                                                                                                                                                                   
        ///</summary>
        public static readonly Guid TopicalApplicationIntraluminal = Guid.Parse("1E204AB0-C59B-4F07-9DBC-95C457D2DE76");
        /// <summary> Injection, Intralymphatic                                                                                                                                                                                                                                                                                                                                                   
        ///</summary>
        public static readonly Guid InjectionIntralymphatic = Guid.Parse("84F02B3F-E169-48A5-86CC-1E8BDD56143F");
        /// <summary> Injection, Intramuscular                                                                                                                                                                                                                                                                                                                                                     
        ///</summary>
        public static readonly Guid InjectionIntramuscular = Guid.Parse("D594F99F-0151-41A0-A359-282AB54683A1");
        /// <summary> Injection, Intramuscular, deep                                                                                                                                                                                                                                                                                                                                          
        ///</summary>
        public static readonly Guid InjectionIntramuscularDeep = Guid.Parse("123F9247-AC29-4703-99B3-799F4E246001");
        /// <summary> Injection, Intramuscular, z Track                                                                                                                                                                                                                                                                                                                                    
        ///</summary>
        public static readonly Guid InjectionIntramuscularZTrack = Guid.Parse("87CBC7F5-CBAB-4B36-B40E-3D85ED1EFD15");
        /// <summary> Injection, Intramedullary                                                                                                                                                                                                                                                                                                                                                   
        ///</summary>
        public static readonly Guid InjectionIntramedullary = Guid.Parse("4EF92918-B665-408D-9AFD-EDC65FDFA521");
        /// <summary> Insufflation                                                                                                                                                                                                                                                                                                                                                                            
        ///</summary>
        public static readonly Guid Insufflation = Guid.Parse("4CC44471-A2B5-412D-B505-145A346BACDD");
        /// <summary> Injection, Interameningeal                                                                                                                                                                                                                                                                                                                                                 
        ///</summary>
        public static readonly Guid InjectionInterameningeal = Guid.Parse("038443D6-2484-450C-BD5C-91E12FF1A0A6");
        /// <summary> Injection, Interstitial                                                                                                                                                                                                                                                                                                                                                       
        ///</summary>
        public static readonly Guid InjectionInterstitial = Guid.Parse("AED81A64-EC65-44EA-B4BF-646417889837");
        /// <summary> Injection, Intraocular                                                                                                                                                                                                                                                                                                                                                         
        ///</summary>
        public static readonly Guid InjectionIntraocular = Guid.Parse("AD78DAA6-5882-4B11-ADB5-A2637A3931ED");
        /// <summary> Instillation, Intraocular                                                                                                                                                                                                                                                                                                                                                   
        ///</summary>
        public static readonly Guid InstillationIntraocular = Guid.Parse("FCF07C1D-F3BC-4F07-A9DD-D09516B7C5DC");
        /// <summary> Irrigation, Intraocular                                                                                                                                                                                                                                                                                                                                                       
        ///</summary>
        public static readonly Guid IrrigationIntraocular = Guid.Parse("5CCF45A4-A7F9-4A8E-B3A6-B23B3EB4561C");
        /// <summary> Topical application, Iontophoresis                                                                                                                                                                                                                                                                                                                                 
        ///</summary>
        public static readonly Guid TopicalApplicationIontophoresis = Guid.Parse("4235AC69-95D6-4C86-9BDD-8F86F22C6608");
        /// <summary> Infusion, Intraosseous, Continuous                                                                                                                                                                                                                                                                                                                                  
        ///</summary>
        public static readonly Guid InfusionIntraosseousContinuous = Guid.Parse("52A9B5C7-3F3A-4EF6-9573-224A37D7B9E9");
        /// <summary> Injection, Intraosseous                                                                                                                                                                                                                                                                                                                                                       
        ///</summary>
        public static readonly Guid InjectionIntraosseous = Guid.Parse("3520664B-9CFF-4008-ACC1-15CF0D80B912");
        /// <summary> Insertion, Intraocular, surgical                                                                                                                                                                                                                                                                                                                                      
        ///</summary>
        public static readonly Guid InsertionIntraocularSurgical = Guid.Parse("3A54C8FD-629E-4062-9A1F-2888A607AFBF");
        /// <summary> Topical application, Intraocular                                                                                                                                                                                                                                                                                                                                     
        ///</summary>
        public static readonly Guid TopicalApplicationIntraocular = Guid.Parse("6FC1408B-7FF2-457F-8B04-9DD997D4E606");
        /// <summary> Injection, Intraovarian                                                                                                                                                                                                                                                                                                                                                       
        ///</summary>
        public static readonly Guid InjectionIntraovarian = Guid.Parse("A7EEEC0F-034F-4EB2-B086-F6F5F357A118");
        /// <summary> Injection, Intrapericardial                                                                                                                                                                                                                                                                                                                                               
        ///</summary>
        public static readonly Guid InjectionIntrapericardial = Guid.Parse("80239A6A-4090-453B-A3DA-0CA689181338");
        /// <summary> Injection, Intraperitoneal                                                                                                                                                                                                                                                                                                                                                 
        ///</summary>
        public static readonly Guid InjectionIntraperitoneal = Guid.Parse("4114066E-EF1A-4A58-8C4F-5F01092D8DFA");
        /// <summary> Injection, Intrapulmonary                                                                                                                                                                                                                                                                                                                                                   
        ///</summary>
        public static readonly Guid InjectionIntrapulmonary = Guid.Parse("23B04119-AAFE-4AD3-AB07-99C97515DABA");
        /// <summary> Injection, Intrapleural                                                                                                                                                                                                                                                                                                                                                       
        ///</summary>
        public static readonly Guid InjectionIntrapleural = Guid.Parse("F84C6E3C-58AC-4EE9-8BF6-186ACF9E3140");
        /// <summary> Inhalation, Intermittent positive pressure breathing (ippb)                                                                                                                                                                                                                                                                               
        ///</summary>
        public static readonly Guid InhalationIntermittentPositivePressureBreathingIppb = Guid.Parse("87128555-8AD4-437F-8A68-CADC70BE8F83");
        /// <summary> Injection, Intraprostatic                                                                                                                                                                                                                                                                                                                                                   
        ///</summary>
        public static readonly Guid InjectionIntraprostatic = Guid.Parse("E073522D-2726-4237-A0FC-10FF49125348");
        /// <summary> Injection, Insulin pump                                                                                                                                                                                                                                                                                                                                                       
        ///</summary>
        public static readonly Guid InjectionInsulinPump = Guid.Parse("76D19146-7C46-443A-AF9C-1D8FDF06FAA3");
        /// <summary> Instillation, Intrasinal                                                                                                                                                                                                                                                                                                                                                     
        ///</summary>
        public static readonly Guid InstillationIntrasinal = Guid.Parse("C45FC084-7B68-4CF5-B490-F7665D28CDDF");
        /// <summary> Injection, Intraspinal                                                                                                                                                                                                                                                                                                                                                         
        ///</summary>
        public static readonly Guid InjectionIntraspinal = Guid.Parse("209452BC-E55B-460C-8365-36F31E3A87C0");
        /// <summary> Injection, Intrasternal                                                                                                                                                                                                                                                                                                                                                       
        ///</summary>
        public static readonly Guid InjectionIntrasternal = Guid.Parse("265F1E47-0710-465D-97AE-9B6140ED17CE");
        /// <summary> Injection, Intrasynovial                                                                                                                                                                                                                                                                                                                                                     
        ///</summary>
        public static readonly Guid InjectionIntrasynovial = Guid.Parse("75989133-9647-424B-935E-76DD6F6F83B0");
        /// <summary> Infusion, Intrathecal                                                                                                                                                                                                                                                                                                                                                           
        ///</summary>
        public static readonly Guid InfusionIntrathecal = Guid.Parse("F0F4C7A5-7058-4A9B-ADC6-7247E0A4D3B0");
        /// <summary> Injection, Intratendinous                                                                                                                                                                                                                                                                                                                                                   
        ///</summary>
        public static readonly Guid InjectionIntratendinous = Guid.Parse("6BAB606E-AA01-430F-BBEC-4AAF6CA8D648");
        /// <summary> Injection, Intratesticular                                                                                                                                                                                                                                                                                                                                                 
        ///</summary>
        public static readonly Guid InjectionIntratesticular = Guid.Parse("6973C8EA-9488-4F6B-AFF0-37E8C2E71F9E");
        /// <summary> Injection, Intrathoracic                                                                                                                                                                                                                                                                                                                                                     
        ///</summary>
        public static readonly Guid InjectionIntrathoracic = Guid.Parse("6B9F1321-1939-4817-A3DB-F02647D402B5");
        /// <summary> Injection, Intrathecal                                                                                                                                                                                                                                                                                                                                                         
        ///</summary>
        public static readonly Guid InjectionIntrathecal = Guid.Parse("47648A25-10D9-4E45-8E97-37FC08556E84");
        /// <summary> Instillation, Intratracheal                                                                                                                                                                                                                                                                                                                                               
        ///</summary>
        public static readonly Guid InstillationIntratracheal = Guid.Parse("C8D422CA-CC22-4071-8216-7703B4ADF66E");
        /// <summary> mucosal absorption, Intratracheal                                                                                                                                                                                                                                                                                                                                   
        ///</summary>
        public static readonly Guid MucosalAbsorptionIntratracheal = Guid.Parse("148A1B72-A326-43DA-9F97-1D28FA26B7D3");
        /// <summary> Injection, Intratubular                                                                                                                                                                                                                                                                                                                                                       
        ///</summary>
        public static readonly Guid InjectionIntratubular = Guid.Parse("6BFF905A-8798-4534-9DF4-69EBD9C60834");
        /// <summary> Injection, Intratumor                                                                                                                                                                                                                                                                                                                                                           
        ///</summary>
        public static readonly Guid InjectionIntratumor = Guid.Parse("AA21C4E1-E417-4824-84EF-AE54ED20DC7D");
        /// <summary> Injection, Intratympanic                                                                                                                                                                                                                                                                                                                                                     
        ///</summary>
        public static readonly Guid InjectionIntratympanic = Guid.Parse("C3FF3725-739E-4DA0-998A-BBA81EB8438F");
        /// <summary> Insertion, Intrauterine                                                                                                                                                                                                                                                                                                                                                       
        ///</summary>
        public static readonly Guid InsertionIntrauterine = Guid.Parse("8997D092-946A-4973-A9F2-535ED383A97D");
        /// <summary> Injection, Intrauterine                                                                                                                                                                                                                                                                                                                                                       
        ///</summary>
        public static readonly Guid InjectionIntrauterine = Guid.Parse("8EA67262-B758-45AD-9058-32CA791B1666");
        /// <summary> Injection, Intracervical (uterus)                                                                                                                                                                                                                                                                                                                                   
        ///</summary>
        public static readonly Guid InjectionIntracervicalUterus = Guid.Parse("06DABE80-8F0B-4C01-A130-F2DE3C8019E3");
        /// <summary> Instillation, Intrauterine                                                                                                                                                                                                                                                                                                                                                 
        ///</summary>
        public static readonly Guid InstillationIntrauterine = Guid.Parse("8914DB6F-EEA3-4F73-A570-C40AEADB51D3");
        /// <summary> Injection, Intraureteral, retrograde                                                                                                                                                                                                                                                                                                                              
        ///</summary>
        public static readonly Guid InjectionIntraureteralRetrograde = Guid.Parse("10B8BB77-8EBA-4B45-BA17-7FCBEF32B004");
        /// <summary> Infusion, Intravenous                                                                                                                                                                                                                                                                                                                                                           
        ///</summary>
        public static readonly Guid InfusionIntravenous = Guid.Parse("2BD1E75C-D111-4445-A217-03E9A7CCF72D");
        /// <summary> Infusion, Intravenous Catheter                                                                                                                                                                                                                                                                                                                                         
        ///</summary>
        public static readonly Guid InfusionIntravenousCatheter = Guid.Parse("820C30AF-65D8-437D-925C-C9476BC1E419");
        /// <summary> Infusion, Intravenous Catheter, Continuous                                                                                                                                                                                                                                                                                                                  
        ///</summary>
        public static readonly Guid InfusionIntravenousCatheterContinuous = Guid.Parse("9D468457-1943-4195-A0D7-4A1D44E11C89");
        /// <summary> Infusion, Intravenous Catheter, Intermittent                                                                                                                                                                                                                                                                                                              
        ///</summary>
        public static readonly Guid InfusionIntravenousCatheterIntermittent = Guid.Parse("A94D1249-1258-48BD-B568-9E210155F876");
        /// <summary> Infusion, Intravenous Catheter, pca pump                                                                                                                                                                                                                                                                                                                      
        ///</summary>
        public static readonly Guid InfusionIntravenousCatheterPcaPump = Guid.Parse("EF81891F-C05E-4B8A-8C20-35058B7E1178");
        /// <summary> Infusion, Intravascular                                                                                                                                                                                                                                                                                                                                                       
        ///</summary>
        public static readonly Guid InfusionIntravascular = Guid.Parse("21C4EF6B-0EB3-4C0D-8782-A6F030990684");
        /// <summary> Injection, Intravascular                                                                                                                                                                                                                                                                                                                                                     
        ///</summary>
        public static readonly Guid InjectionIntravascular = Guid.Parse("E232435D-FCE2-4160-8843-549F81053486");
        /// <summary> Injection, Intraventricular (heart)                                                                                                                                                                                                                                                                                                                               
        ///</summary>
        public static readonly Guid InjectionIntraventricularHeart = Guid.Parse("6F1661DC-ABB5-461C-85DA-35C483406E59");
        /// <summary> Injection, Intravesicle                                                                                                                                                                                                                                                                                                                                                       
        ///</summary>
        public static readonly Guid InjectionIntravesicle = Guid.Parse("11DFE105-32A0-495D-BF5E-06C9C00F5AF6");
        /// <summary> flush, Intravenous Catheter                                                                                                                                                                                                                                                                                                                                               
        ///</summary>
        public static readonly Guid FlushIntravenousCatheter = Guid.Parse("D8EF2E83-4C8A-4409-8908-ACA17D9F7F9E");
        /// <summary> Injection, Intravenous                                                                                                                                                                                                                                                                                                                                                         
        ///</summary>
        public static readonly Guid InjectionIntravenous = Guid.Parse("ACC0F611-EBC8-4A15-9DB5-519F06024CEF");
        /// <summary> Injection, Intravenous, bolus                                                                                                                                                                                                                                                                                                                                            
        ///</summary>
        public static readonly Guid InjectionIntravenousBolus = Guid.Parse("000A07E2-87B0-455A-A986-F6420C6FAC82");
        /// <summary> Injection, Intravenous, push                                                                                                                                                                                                                                                                                                                                              
        ///</summary>
        public static readonly Guid InjectionIntravenousPush = Guid.Parse("6F544C50-14DF-4B41-BECD-40D889BAB557");
        /// <summary> Injection, Intravenous, rapid push                                                                                                                                                                                                                                                                                                                                  
        ///</summary>
        public static readonly Guid InjectionIntravenousRapidPush = Guid.Parse("E95332F5-7B16-4574-A760-892F9E1649D5");
        /// <summary> Injection, Intravenous, slow push                                                                                                                                                                                                                                                                                                                                    
        ///</summary>
        public static readonly Guid InjectionIntravenousSlowPush = Guid.Parse("323882FF-E4AB-426C-B71E-AB2DDF93D5B9");
        /// <summary> Implantation, Intravitreal                                                                                                                                                                                                                                                                                                                                                 
        ///</summary>
        public static readonly Guid ImplantationIntravitreal = Guid.Parse("96507D23-C70F-4C05-8A26-5A0BEF59C24C");
        /// <summary> Injection, Intravitreal                                                                                                                                                                                                                                                                                                                                                       
        ///</summary>
        public static readonly Guid InjectionIntravitreal = Guid.Parse("1C30DED0-8314-47D6-B208-B72831F48807");
        /// <summary> Instillation, jejunostomy Tube                                                                                                                                                                                                                                                                                                                                         
        ///</summary>
        public static readonly Guid InstillationJejunostomyTube = Guid.Parse("DE55B502-71E1-4F5F-98A3-9A71D2828DD4");
        /// <summary> Instillation, laryngeal                                                                                                                                                                                                                                                                                                                                                       
        ///</summary>
        public static readonly Guid InstillationLaryngeal = Guid.Parse("7D33BC7F-4B25-4D87-A780-65331B088F58");
        /// <summary> Topical application, laryngeal                                                                                                                                                                                                                                                                                                                                         
        ///</summary>
        public static readonly Guid TopicalApplicationLaryngeal = Guid.Parse("FB5F4877-C5FB-496C-8E0B-CD7042B58F9B");
        /// <summary> Insertion, lacrimal puncta                                                                                                                                                                                                                                                                                                                                                 
        ///</summary>
        public static readonly Guid InsertionLacrimalPuncta = Guid.Parse("A9276C74-8818-4D70-9DDC-EE4A506B60B1");
        /// <summary> Topical application, mucous membrane                                                                                                                                                                                                                                                                                                                             
        ///</summary>
        public static readonly Guid TopicalApplicationMucousMembrane = Guid.Parse("3B57901B-7104-4298-B559-4100EFE553F3");
        /// <summary> Topical application, nail                                                                                                                                                                                                                                                                                                                                                   
        ///</summary>
        public static readonly Guid TopicalApplicationNail = Guid.Parse("8F1A1CD5-FCE3-40FD-91E9-62049E5F6A94");
        /// <summary> Topical application, nasal                                                                                                                                                                                                                                                                                                                                                 
        ///</summary>
        public static readonly Guid TopicalApplicationNasal = Guid.Parse("128AD7A8-76D9-401F-BE1A-33184E3DE40D");
        /// <summary> Instillation, nasal                                                                                                                                                                                                                                                                                                                                                               
        ///</summary>
        public static readonly Guid InstillationNasal = Guid.Parse("9E064118-48A3-4EC3-8F42-9C81E3149A88");
        /// <summary> Inhalation, nasal                                                                                                                                                                                                                                                                                                                                                                   
        ///</summary>
        public static readonly Guid InhalationNasal = Guid.Parse("8B5B870D-919D-47F0-8EC1-F85C2AB2CE10");
        /// <summary> Inhalation, nasal Cannula                                                                                                                                                                                                                                                                                                                                                   
        ///</summary>
        public static readonly Guid InhalationNasalCannula = Guid.Parse("5BB7FCAC-23FE-4818-B1FB-083272F8F993");
        /// <summary> Instillation, nasogastric                                                                                                                                                                                                                                                                                                                                                   
        ///</summary>
        public static readonly Guid InstillationNasogastric = Guid.Parse("FE1DAEE9-44F6-421D-881F-C46C55C17644");
        /// <summary> Inhalation, nebulization                                                                                                                                                                                                                                                                                                                                                     
        ///</summary>
        public static readonly Guid InhalationNebulization = Guid.Parse("5FCFFD0C-B4B2-47E6-984A-FF2C4A342B0F");
        /// <summary> Inhalation, nebulization, nasal                                                                                                                                                                                                                                                                                                                                        
        ///</summary>
        public static readonly Guid InhalationNebulizationNasal = Guid.Parse("BDA4566C-4CAA-41DD-8318-EFF99AC98971");
        /// <summary> Inhalation, nebulization, oral                                                                                                                                                                                                                                                                                                                                          
        ///</summary>
        public static readonly Guid InhalationNebulizationOral = Guid.Parse("206EF13E-F4E4-4A86-8E35-D2C8AD112CA3");
        /// <summary> Instillation, nasogastric Tube                                                                                                                                                                                                                                                                                                                                         
        ///</summary>
        public static readonly Guid InstillationNasogastricTube = Guid.Parse("8E239E95-C918-4516-89C1-E9AA40F36E9F");
        /// <summary> Instillation, nasotracheal Tube                                                                                                                                                                                                                                                                                                                                       
        ///</summary>
        public static readonly Guid InstillationNasotrachealTube = Guid.Parse("825CCBF5-0086-4BB9-91D1-DABB0B290E0E");
        /// <summary> occlusive dressing Technique                                                                                                                                                                                                                                                                                                                                            
        ///</summary>
        public static readonly Guid OcclusiveDressingTechnique = Guid.Parse("C99C71F9-C3D8-4024-9332-8EECE4920C9C");
        /// <summary> Instillation, orogastric Tube                                                                                                                                                                                                                                                                                                                                           
        ///</summary>
        public static readonly Guid InstillationOrogastricTube = Guid.Parse("EC25D0AE-9C47-4F8D-B1B5-DA004F87F0E6");
        /// <summary> Instillation, orojejunum Tube                                                                                                                                                                                                                                                                                                                                           
        ///</summary>
        public static readonly Guid InstillationOrojejunumTube = Guid.Parse("D3F5A428-0146-4CCB-AD6F-EC62233EB1D9");
        /// <summary> Topical application, ophthalmic                                                                                                                                                                                                                                                                                                                                       
        ///</summary>
        public static readonly Guid TopicalApplicationOphthalmic = Guid.Parse("172067EF-D4BA-406D-B59D-722EC37AD285");
        /// <summary> Topical application, oral                                                                                                                                                                                                                                                                                                                                                   
        ///</summary>
        public static readonly Guid TopicalApplicationOral = Guid.Parse("FCA8070B-01BB-427F-ADBB-A048171858A9");
        /// <summary> Inhalation, respiratory                                                                                                                                                                                                                                                                                                                                                       
        ///</summary>
        public static readonly Guid InhalationRespiratory = Guid.Parse("3CF80B24-96CD-44F2-94B6-2DFF7AC3AFA8");
        /// <summary> Inhalation, oral Intermittent flow                                                                                                                                                                                                                                                                                                                                 
        ///</summary>
        public static readonly Guid InhalationOralIntermittentFlow = Guid.Parse("1B4E469D-FFA0-4B2E-8567-F2E563D34D04");
        /// <summary> Inhalation, oral rebreather mask                                                                                                                                                                                                                                                                                                                                     
        ///</summary>
        public static readonly Guid InhalationOralRebreatherMask = Guid.Parse("2FAA29AF-FCDF-49EE-A303-129417425A78");
        /// <summary> Topical application, oromucosal                                                                                                                                                                                                                                                                                                                                       
        ///</summary>
        public static readonly Guid TopicalApplicationOromucosal = Guid.Parse("2B62D94B-D60E-491D-AD93-C6366100FCBA");
        /// <summary> Topical application, oropharyngeal                                                                                                                                                                                                                                                                                                                                 
        ///</summary>
        public static readonly Guid TopicalApplicationOropharyngeal = Guid.Parse("0995DAFF-81E0-4A5B-8767-B0BEDE28960C");
        /// <summary> Inse, oral                                                                                                                                                                                                                                                                                                                                                                                 
        ///</summary>
        public static readonly Guid InseOral = Guid.Parse("3649FB62-7D17-4E35-AB1B-4153653C69BB");
        /// <summary> Instillation, otic                                                                                                                                                                                                                                                                                                                                                                 
        ///</summary>
        public static readonly Guid InstillationOtic = Guid.Parse("AB7C6723-C7DD-4C83-9304-AADCA3A56EA1");
        /// <summary> Injection, periarticular                                                                                                                                                                                                                                                                                                                                                     
        ///</summary>
        public static readonly Guid InjectionPeriarticular = Guid.Parse("A6D70D6E-7C7D-4F8B-872D-1BB8A5E2E7AB");
        /// <summary> Injection, parenteral                                                                                                                                                                                                                                                                                                                                                           
        ///</summary>
        public static readonly Guid InjectionParenteral = Guid.Parse("A7AA192D-4CAB-45CD-9AC3-97E40E7E0BB0");
        /// <summary> Injection, periodontal                                                                                                                                                                                                                                                                                                                                                         
        ///</summary>
        public static readonly Guid InjectionPeriodontal = Guid.Parse("9D0FBA07-46DC-45DE-BCB6-5AA8820AC772");
        /// <summary> Topical application, periodontal                                                                                                                                                                                                                                                                                                                                     
        ///</summary>
        public static readonly Guid TopicalApplicationPeriodontal = Guid.Parse("D2A8D599-38CE-46E3-88EB-FADA73CA431D");
        /// <summary> Injection, peritoneal dialysis port                                                                                                                                                                                                                                                                                                                               
        ///</summary>
        public static readonly Guid InjectionPeritonealDialysisPort = Guid.Parse("7A4389EE-5FEF-4FA2-997E-155913389AFD");
        /// <summary> Instillation, peritoneal dialysis port                                                                                                                                                                                                                                                                                                                         
        ///</summary>
        public static readonly Guid InstillationPeritonealDialysisPort = Guid.Parse("2DDA32D5-7FDE-4946-BAA1-DBEC6B364A98");
        /// <summary> Injection, peridural                                                                                                                                                                                                                                                                                                                                                             
        ///</summary>
        public static readonly Guid InjectionPeridural = Guid.Parse("5CE2DF2C-0195-47FF-87FA-3EBE9D8084EA");
        /// <summary> Topical application, perianal                                                                                                                                                                                                                                                                                                                                           
        ///</summary>
        public static readonly Guid TopicalApplicationPerianal = Guid.Parse("A4A20BFC-26E4-405D-9085-7C101EB05B29");
        /// <summary> Topical application, perineal                                                                                                                                                                                                                                                                                                                                           
        ///</summary>
        public static readonly Guid TopicalApplicationPerineal = Guid.Parse("D222003D-5623-4A7C-8915-AEBACC8C2644");
        /// <summary> Injection, perineural                                                                                                                                                                                                                                                                                                                                                           
        ///</summary>
        public static readonly Guid InjectionPerineural = Guid.Parse("1B40CBCC-EFD5-4DE8-9272-5CB78E2817F5");
        /// <summary> Injection, paranasal sinuses                                                                                                                                                                                                                                                                                                                                             
        ///</summary>
        public static readonly Guid InjectionParanasalSinuses = Guid.Parse("F8659096-E688-4AE5-97DD-D2D255EBD27C");
        /// <summary> Instillation, paranasal sinuses                                                                                                                                                                                                                                                                                                                                       
        ///</summary>
        public static readonly Guid InstillationParanasalSinuses = Guid.Parse("5DC17CDA-EE65-4778-9475-E93D22D9B063");
        /// <summary> swallow, oral                                                                                                                                                                                                                                                                                                                                                                           
        ///</summary>
        public static readonly Guid SwallowOral = Guid.Parse("0A1388B0-66FB-4063-BFE3-151DD8442838");
        /// <summary> Insertion, rectal                                                                                                                                                                                                                                                                                                                                                                   
        ///</summary>
        public static readonly Guid InsertionRectal = Guid.Parse("E53223AD-EC4F-4838-AD17-0F4D0201BC2B");
        /// <summary> Injection, retrobulbar                                                                                                                                                                                                                                                                                                                                                         
        ///</summary>
        public static readonly Guid InjectionRetrobulbar = Guid.Parse("167EA617-A8E2-45F3-B902-B5BD68502C75");
        /// <summary> Instillation, rectal                                                                                                                                                                                                                                                                                                                                                             
        ///</summary>
        public static readonly Guid InstillationRectal = Guid.Parse("5B6A61D0-0454-4FDA-A0FC-935E4244B300");
        /// <summary> Instillation, rectal Tube                                                                                                                                                                                                                                                                                                                                                   
        ///</summary>
        public static readonly Guid InstillationRectalTube = Guid.Parse("102F4242-C33F-4699-8410-1F55438E435C");
        /// <summary> Irrigation, rectal                                                                                                                                                                                                                                                                                                                                                                 
        ///</summary>
        public static readonly Guid IrrigationRectal = Guid.Parse("8CF2158B-2145-4C95-9C9F-79623CF07146");
        /// <summary> Topical application, rectal                                                                                                                                                                                                                                                                                                                                               
        ///</summary>
        public static readonly Guid TopicalApplicationRectal = Guid.Parse("B3E1D26D-FA90-4DDA-88E4-3CBEA4A59C25");
        /// <summary> enema, rectal retention                                                                                                                                                                                                                                                                                                                                                       
        ///</summary>
        public static readonly Guid EnemaRectalRetention = Guid.Parse("B7B315A5-0EAC-41BA-842C-A0C072AA7A96");
        /// <summary> Topical application, scalp                                                                                                                                                                                                                                                                                                                                                 
        ///</summary>
        public static readonly Guid TopicalApplicationScalp = Guid.Parse("75910BCE-E355-423A-9DF1-159176039F14");
        /// <summary> Injection, subconjunctival                                                                                                                                                                                                                                                                                                                                                 
        ///</summary>
        public static readonly Guid InjectionSubconjunctival = Guid.Parse("CC966B06-F925-4FE5-8496-871A8D078AE1");
        /// <summary> shampoo                                                                                                                                                                                                                                                                                                                                                                                      
        ///</summary>
        public static readonly Guid Shampoo = Guid.Parse("CD738DC7-A6EB-4701-AD2B-FE72E0FC0187");
        /// <summary> Instillation, sinus, unspecified                                                                                                                                                                                                                                                                                                                                      
        ///</summary>
        public static readonly Guid InstillationSinusUnspecified = Guid.Parse("83BFED76-387A-4B2E-879C-B2639FF05ED4");
        /// <summary> Topical application, skin                                                                                                                                                                                                                                                                                                                                                   
        ///</summary>
        public static readonly Guid TopicalApplicationSkin = Guid.Parse("DE531D3B-4BB9-4B58-8132-688B58D07C8F");
        /// <summary> dissolve, sublingual                                                                                                                                                                                                                                                                                                                                                             
        ///</summary>
        public static readonly Guid DissolveSublingual = Guid.Parse("2B28231B-0675-4590-9EAB-9F109026A9AE");
        /// <summary> Injection, sublesional                                                                                                                                                                                                                                                                                                                                                         
        ///</summary>
        public static readonly Guid InjectionSublesional = Guid.Parse("CAA21FDD-A465-4570-8108-B227409B587D");
        /// <summary> mucosal absorption, submucosal                                                                                                                                                                                                                                                                                                                                         
        ///</summary>
        public static readonly Guid MucosalAbsorptionSubmucosal = Guid.Parse("CA6CE9F7-641D-43DA-ADBD-776AC2012787");
        /// <summary> Immersion (soak)                                                                                                                                                                                                                                                                                                                                                                    
        ///</summary>
        public static readonly Guid ImmersionSoak = Guid.Parse("979D4BD0-7C96-45B3-BB5D-5ADEB74E55F0");
        /// <summary> Injection, soft Tissue                                                                                                                                                                                                                                                                                                                                                         
        ///</summary>
        public static readonly Guid InjectionSoftTissue = Guid.Parse("C44DDB27-A966-448E-B699-A6A3D5B227CD");
        /// <summary> Instillation, soft Tissue                                                                                                                                                                                                                                                                                                                                                   
        ///</summary>
        public static readonly Guid InstillationSoftTissue = Guid.Parse("D1209084-9C5A-4330-9987-67CB8269F47C");
        /// <summary> Injection, subcutaneous                                                                                                                                                                                                                                                                                                                                                       
        ///</summary>
        public static readonly Guid InjectionSubcutaneous = Guid.Parse("B6BC99B9-BF22-4DC1-99E5-DD55925F3D63");
        /// <summary> Implantation, subcutaneous                                                                                                                                                                                                                                                                                                                                                 
        ///</summary>
        public static readonly Guid ImplantationSubcutaneous = Guid.Parse("1C535D51-AC88-49DF-B721-B7C0610890EC");
        /// <summary> Infusion, subcutaneous                                                                                                                                                                                                                                                                                                                                                         
        ///</summary>
        public static readonly Guid InfusionSubcutaneous = Guid.Parse("CEF5E03E-BEEA-437C-91E2-66D24C1476D3");
        /// <summary> Insertion, subcutaneous, surgical                                                                                                                                                                                                                                                                                                                                    
        ///</summary>
        public static readonly Guid InsertionSubcutaneousSurgical = Guid.Parse("15AD9375-75A6-486C-A852-E551874955D8");
        /// <summary> Injection, subarachnoid                                                                                                                                                                                                                                                                                                                                                       
        ///</summary>
        public static readonly Guid InjectionSubarachnoid = Guid.Parse("35323049-F958-4B4C-A92D-49B000C0B9CD");
        /// <summary> subconjunctival                                                                                                                                                                                                                                                                                                                                                                      
        ///</summary>
        public static readonly Guid Subconjunctival = Guid.Parse("689E808B-23A5-4E6F-BF78-25BB76C04A52");
        /// <summary> Injection, submucosal                                                                                                                                                                                                                                                                                                                                                           
        ///</summary>
        public static readonly Guid InjectionSubmucosal = Guid.Parse("DDDD2DA6-B53B-438F-B93F-9C47CA05035A");
        /// <summary> suck, oromucosal                                                                                                                                                                                                                                                                                                                                                                     
        ///</summary>
        public static readonly Guid SuckOromucosal = Guid.Parse("372E896E-EBD6-4D2B-B424-C3C1643E23F1");
        /// <summary> Topical application, swab                                                                                                                                                                                                                                                                                                                                                   
        ///</summary>
        public static readonly Guid TopicalApplicationSwab = Guid.Parse("B79C336A-1A8A-4EEF-A87C-640FDED62B61");
        /// <summary> swish and Spit out, oromucosal                                                                                                                                                                                                                                                                                                                                         
        ///</summary>
        public static readonly Guid SwishAndSpitOutOromucosal = Guid.Parse("AA6221E3-83BA-49D4-913C-0A07C8A6DCFF");
        /// <summary> swish and Swallow, oromucosal                                                                                                                                                                                                                                                                                                                                           
        ///</summary>
        public static readonly Guid SwishAndSwallowOromucosal = Guid.Parse("C18A0579-953F-4A90-A333-911428F3E052");
        /// <summary> Topical application, Transmucosal                                                                                                                                                                                                                                                                                                                                   
        ///</summary>
        public static readonly Guid TopicalApplicationTransmucosal = Guid.Parse("1B859971-1E63-4C87-AADA-D03C68816C9B");
        /// <summary> Topical                                                                                                                                                                                                                                                                                                                                                                                      
        ///</summary>
        public static readonly Guid Topical = Guid.Parse("FD519D46-8219-4FF3-918B-E2EE28C03587");
        /// <summary> Inhalation, Tracheostomy                                                                                                                                                                                                                                                                                                                                                     
        ///</summary>
        public static readonly Guid InhalationTracheostomy = Guid.Parse("FFFA65E4-C8B9-45CD-8A88-511A55FE7D88");
        /// <summary> Instillation, Tracheostomy                                                                                                                                                                                                                                                                                                                                                 
        ///</summary>
        public static readonly Guid InstillationTracheostomy = Guid.Parse("4FAB2729-3103-4334-B068-608D1F4B8238");
        /// <summary> Transdermal                                                                                                                                                                                                                                                                                                                                                                              
        ///</summary>
        public static readonly Guid Transdermal = Guid.Parse("F410DDEF-6827-425C-9456-81B6F67460A5");
        /// <summary> diffusion, Transdermal                                                                                                                                                                                                                                                                                                                                                         
        ///</summary>
        public static readonly Guid DiffusionTransdermal = Guid.Parse("03196186-CDE1-4DCF-9677-F1C418A5ECB6");
        /// <summary> Translingual                                                                                                                                                                                                                                                                                                                                                                            
        ///</summary>
        public static readonly Guid Translingual = Guid.Parse("7FBB9108-7893-4F47-99E2-3DDD79D6A884");
        /// <summary> Injection, Transplacental                                                                                                                                                                                                                                                                                                                                                   
        ///</summary>
        public static readonly Guid InjectionTransplacental = Guid.Parse("36F5495F-B250-4C7B-A620-E01F67DAABD3");
        /// <summary> Injection, Transtracheal                                                                                                                                                                                                                                                                                                                                                     
        ///</summary>
        public static readonly Guid InjectionTranstracheal = Guid.Parse("E6D3E8E2-CA42-479D-A71E-05F9B8DC42D2");
        /// <summary> Instillation, Transtympanic                                                                                                                                                                                                                                                                                                                                               
        ///</summary>
        public static readonly Guid InstillationTranstympanic = Guid.Parse("63FA56F9-1D16-49BF-954C-1B0F4B35EDAD");
        /// <summary> Topical absorption, Transtympanic                                                                                                                                                                                                                                                                                                                                   
        ///</summary>
        public static readonly Guid TopicalAbsorptionTranstympanic = Guid.Parse("AB04CC66-F852-458C-82C6-9F5B4D10DD97");
        /// <summary> Injection, urethral                                                                                                                                                                                                                                                                                                                                                               
        ///</summary>
        public static readonly Guid InjectionUrethral = Guid.Parse("55A77F64-93DA-436A-9835-EB5313D641DE");
        /// <summary> Insertion, urethral                                                                                                                                                                                                                                                                                                                                                               
        ///</summary>
        public static readonly Guid InsertionUrethral = Guid.Parse("772263B2-17F4-434E-9703-59A818CC227C");
        /// <summary> Instillation, urethral                                                                                                                                                                                                                                                                                                                                                         
        ///</summary>
        public static readonly Guid InstillationUrethral = Guid.Parse("D19F2EA6-EAE5-4F2B-8EF5-75DC122585CC");
        /// <summary> suppository, urethral                                                                                                                                                                                                                                                                                                                                                           
        ///</summary>
        public static readonly Guid SuppositoryUrethral = Guid.Parse("4400FE11-85C4-49C2-BB6B-3D4E6715CC53");
        /// <summary> Injection, ureteral                                                                                                                                                                                                                                                                                                                                                               
        ///</summary>
        public static readonly Guid InjectionUreteral = Guid.Parse("B4AAAE01-904D-4BFC-B81D-57CCA06E9443");
        /// <summary> Topical application, vaginal                                                                                                                                                                                                                                                                                                                                             
        ///</summary>
        public static readonly Guid TopicalApplicationVaginal = Guid.Parse("99AB25C8-69E0-4FC1-849C-6D730413B643");
        /// <summary> Insertion, vaginal                                                                                                                                                                                                                                                                                                                                                                 
        ///</summary>
        public static readonly Guid InsertionVaginal = Guid.Parse("F88A29A5-75C7-4834-8034-A77D9E961417");
        /// <summary> Inhalation, ventilator   
        ///</summary>
        public static readonly Guid InhalationVentilator = Guid.Parse("4CE3798F-BF05-4032-8E00-A1183132FFE6");
    }
}
