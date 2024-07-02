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
    /// Base entity relationship type keys
    /// </summary>
    public static class EntityRelationshipTypeKeyStrings
    {
        /// <summary>
        /// The source entity gives access to the target entity
        /// </summary>
        public const string Access = "DDC1B705-C768-4C7A-8F69-76AD4B167B40";

        /// <summary>
        /// Active ingredient, where not the ingredient substance (player), but itaTMs active moiety is the "basis of strength"
        /// </summary>
        public const string ActiveMoiety = "212B1B6B-B074-4A75-862D-E4E194252044";

        /// <summary>
        /// The source represents a meterial that is an administerable form of the target
        /// </summary>
        public const string AdministerableMaterial = "B52C7E95-88B8-4C4C-836A-934277AFDB92";

        /// <summary>
        /// The source is an adopted child of the target
        /// </summary>
        public const string AdoptedChild = "8FA25B69-C9C2-4C40-84C1-0EA9641A12EC";

        /// <summary>
        /// The source is an adopted daughter of the target
        /// </summary>
        public const string AdoptedDaughter = "2B4B2ED8-F90C-4193-870A-F48BC39657C1";

        /// <summary>
        /// The source is an adopted son of the target
        /// </summary>
        public const string AdoptedSon = "CE50BA92-CD21-43C4-8582-34E7FBB3170F";

        /// <summary>
        /// The target has a business/professional relationship with the source.
        /// </summary>
        public const string Affiliate = "8DE7B5E7-C941-42BD-B735-52D750EFC5E6";

        /// <summary>
        /// The target is an agent or authorized to act on behalf of the source
        /// </summary>
        public const string Agent = "867FD445-D490-4619-804E-75C04B8A0E57";

        /// <summary>
        /// The target is a portion of the original source
        /// </summary>
        public const string Aliquot = "CFF670E4-965E-4288-B966-47A44479D2AD";

        /// <summary>
        /// The target is an entity acting under the employ of the source entity
        /// </summary>
        public const string AssignedEntity = "77B7A04B-C065-4FAF-8EC0-2CDAD4AE372B";

        /// <summary>
        /// The target is the aunt of the source
        /// </summary>
        public const string Aunt = "0FF2AB03-6E0A-40D1-8947-04C4937B4CC4";

        /// <summary>
        /// The target is the birthplace of the source
        /// </summary>
        public const string Birthplace = "F3EF7E48-D8B7-4030-B431-AFF7E0E1CB76";

        /// <summary>
        /// The target is the brother of the source
        /// </summary>
        public const string Brother = "24380D53-EA22-4820-9F06-8671F774F133";

        /// <summary>
        /// The target is the brotherinlaw of the source
        /// </summary>
        public const string Brotherinlaw = "0A4C87E2-16C3-4361-BE3C-DD765EE4BC7D";

        /// <summary>
        /// The target is the caregiver of the source
        /// </summary>
        public const string Caregiver = "31B0DFCB-D7BA-452A-98B9-45EBCCD30732";

        /// <summary>
        /// The the target represents a case subject of the source entity (such as a study)
        /// </summary>
        public const string CaseSubject = "D7AD48C0-889D-41E2-99E9-BE5E6C5327B2";

        /// <summary>
        /// The the target is a child of the child source
        /// </summary>
        public const string Child = "739457D0-835A-4A9C-811C-42B5E92ED1CA";

        /// <summary>
        /// The child inlaw
        /// </summary>
        public const string ChildInlaw = "8BF23192-DE75-48EB-ABEE-81A9A15332F8";

        /// <summary>
        /// The target is a citizen of the source
        /// </summary>
        public const string Citizen = "35B13152-E43C-4BCB-8649-A9E83BEE33A2";

        /// <summary>
        /// The target is a claimant  or is making a claim in a policy (source)
        /// </summary>
        public const string Claimant = "9D256279-F1AC-46B3-A974-DD13E2AD4F72";

        /// <summary>
        /// The clinical research investigator
        /// </summary>
        public const string ClinicalResearchInvestigator = "43AD7BC0-2ED8-4B27-97E5-B3DB00A07D17";

        /// <summary>
        /// The clinical research sponsor
        /// </summary>
        public const string ClinicalResearchSponsor = "66C96AE6-C5C4-4D66-9BD0-A00C56E831DA";

        /// <summary>
        /// The commissioning party
        /// </summary>
        public const string CommissioningParty = "33BD1401-DFDB-40E7-A914-0A695AD5186E";

        /// <summary>
        /// Community location where health services are delivered
        /// </summary>
        public const string CommunityServiceDeliveryLocation = "4AA573A0-D967-493A-BEA0-8BAD060E4264";

        /// <summary>
        /// The target represents a contact of the source
        /// </summary>
        public const string Contact = "B1D2148D-BB35-4337-8FE6-021F5A3AC8A3";

        /// <summary>
        /// The cousin
        /// </summary>
        public const string Cousin = "1C0F931C-9C49-4A52-8FBF-5217C52EA778";

        /// <summary>
        /// The target represents a coverage sponsor of the source
        /// </summary>
        public const string CoverageSponsor = "8FF9D9A5-A206-4566-82CD-67B770D7CE8A";

        /// <summary>
        /// The target is a covered party of a source (insurance policy)
        /// </summary>
        public const string CoveredParty = "D4844672-C0D7-434C-8377-6DD0655B0532";

        /// <summary>
        /// The daughter
        /// </summary>
        public const string Daughter = "8165B43F-8103-4ED3-BAC6-4FC0DF8C1A84";

        /// <summary>
        /// The daughter inlaw
        /// </summary>
        public const string DaughterInlaw = "76FDF0E7-CFE0-47B4-9630-C645F254CDFD";

        /// <summary>
        /// The target is the dedicated service delivery location for the source
        /// </summary>
        public const string DedicatedServiceDeliveryLocation = "455F1772-F580-47E8-86BD-B5CE25D351F9";

        /// <summary>
        /// The target is a dependent of the source
        /// </summary>
        public const string Dependent = "F28ED78F-85AB-47A1-BA08-B5051E62D6C3";

        /// <summary>
        /// The target is a distributed or shippable material of the source
        /// </summary>
        public const string DistributedMaterial = "F5547ADA-1EB9-40BB-B163-081567D869E7";

        /// <summary>
        /// The domestic partner
        /// </summary>
        public const string DomesticPartner = "3DB182E2-653B-4BFD-A300-32F23345D1C0";

        /// <summary>
        /// The target is an emergency contact for the source
        /// </summary>
        public const string EmergencyContact = "25985F42-476A-4455-A977-4E97A554D710";

        /// <summary>
        /// The the target is an employee of the source
        /// </summary>
        public const string Employee = "B43C9513-1C1C-4ED0-92DB-55A904C122E6";

        /// <summary>
        /// The target represents a substance which is exposed when the source is exposed
        /// </summary>
        public const string ExposedEntity = "AB39087C-17D3-421A-A1E3-2DE4E0AB9FAF";

        /// <summary>
        /// The family member
        /// </summary>
        public const string FamilyMember = "38D66EC7-0CC8-4609-9675-B6FF91EDE605";

        /// <summary>
        /// The father
        /// </summary>
        public const string Father = "40D18ECC-8FF8-4E03-8E58-97A980F04060";

        /// <summary>
        /// The fatherinlaw
        /// </summary>
        public const string Fatherinlaw = "B401DD81-931C-4AAD-8FD8-22A6AC2EA3DC";

        /// <summary>
        /// The foster child
        /// </summary>
        public const string FosterChild = "ABFE2637-D338-4090-B3A5-3EC19A47BE6A";

        /// <summary>
        /// The foster daughter
        /// </summary>
        public const string FosterDaughter = "E81D6773-97E3-4B2D-B6A3-A4624BA5C6A9";

        /// <summary>
        /// The foster son
        /// </summary>
        public const string FosterSon = "DECD6250-7E8B-4B77-895D-31953CF1387A";

        /// <summary>
        /// The grandchild
        /// </summary>
        public const string Grandchild = "C33ADDA2-A4ED-4092-8D9C-B8E3FBD5D90B";

        /// <summary>
        /// The granddaughter
        /// </summary>
        public const string Granddaughter = "3CB1993F-3703-453F-87BE-21B606DB7631";

        /// <summary>
        /// The grandfather
        /// </summary>
        public const string Grandfather = "48C59444-FEC0-43B8-AA2C-7AEDB70733AD";

        /// <summary>
        /// The grandmother
        /// </summary>
        public const string Grandmother = "B630BA2C-8A00-46D8-BF64-870D381D8917";

        /// <summary>
        /// The grandparent
        /// </summary>
        public const string Grandparent = "FA646DF9-7D64-4D1F-AE9A-6261FD5FD6AE";

        /// <summary>
        /// The grandson
        /// </summary>
        public const string Grandson = "F7A64463-BC75-44D4-A8CA-C9FBC2C87175";

        /// <summary>
        /// The great grandfather
        /// </summary>
        public const string GreatGrandfather = "BFE24B5D-9C32-4DF3-AD7B-EAA19E7D4AFB";

        /// <summary>
        /// The great grandmother
        /// </summary>
        public const string GreatGrandmother = "02FBC345-1A25-4F78-AEEA-A12584A1EEC3";

        /// <summary>
        /// The great grandparent
        /// </summary>
        public const string GreatGrandparent = "528FEB11-AE81-426A-BE1F-CE74C83009EB";

        /// <summary>
        /// The guarantor
        /// </summary>
        public const string Guarantor = "F5B10C57-3AE1-41EA-8649-1CF8D9848AE1";

        /// <summary>
        /// The guard
        /// </summary>
        public const string GUARD = "845120DE-E6F7-4CEC-94AA-E6E943C91367";

        /// <summary>
        /// The target is a guardian of the source
        /// </summary>
        public const string Guardian = "3B8E2334-4CCC-4F24-8AAE-37341EA03D3E";

        /// <summary>
        /// The halfbrother
        /// </summary>
        public const string Halfbrother = "25CAE2F2-D1EC-4EFE-A92F-D479785F7D8A";

        /// <summary>
        /// The halfsibling
        /// </summary>
        public const string Halfsibling = "8452ECB9-D762-4C4A-96B2-81D130CB729B";

        /// <summary>
        /// The halfsister
        /// </summary>
        public const string Halfsister = "CE42C680-A783-4CDE-BCD1-E261D6FD68A0";

        /// <summary>
        /// The target is a healthcare provider for the source
        /// </summary>
        public const string HealthcareProvider = "6B04FED8-C164-469C-910B-F824C2BDA4F0";

        /// <summary>
        /// The target represents a health chart belonging to the source
        /// </summary>
        public const string HealthChart = "5B0F8C93-57C9-4DFF-B59A-9564739EF445";

        /// <summary>
        /// The source holds the specified quantity of the target entity (the target entity is held by the source)
        /// </summary>
        public const string HeldEntity = "9C02A621-8565-46B4-94FF-A2BD210989B1";

        /// <summary>
        /// The husband
        /// </summary>
        public const string Husband = "62ACA44C-B57C-44FD-9703-FCDFF97C04B6";

        /// <summary>
        /// The target represents an entity for purposes of identification of the source
        /// </summary>
        public const string IdentifiedEntity = "C5C8B935-294F-4C90-9D81-CBF914BF5808";

        /// <summary>
        /// The target represents an incidental service delivery location related to the source entity
        /// </summary>
        public const string IncidentalServiceDeliveryLocation = "41BAF7AA-5FFD-4421-831F-42D4AB3DE38A";

        /// <summary>
        /// The target represents an individual instance of the source
        /// </summary>
        public const string Individual = "47049B0F-F189-4E19-9AA8-7C38ADB2491A";

        /// <summary>
        /// The investigation subject
        /// </summary>
        public const string InvestigationSubject = "0C522BD1-DFA2-43CB-A98E-F6FF137968AE";

        /// <summary>
        /// The target is the payor of an invoice for the source
        /// </summary>
        public const string InvoicePayor = "07C922D2-12C9-415A-95D4-9B3FED4959D6";

        /// <summary>
        /// The isolate
        /// </summary>
        public const string Isolate = "020C28A0-7C52-42F4-A046-DB9E329D5A42";

        /// <summary>
        /// The target represents an entity licensed to perform or use the source
        /// </summary>
        public const string LicensedEntity = "B9FE057E-7F57-42EB-89D7-67C69646C0C4";

        /// <summary>
        /// The target entity is maintained by the source entity
        /// </summary>
        public const string MaintainedEntity = "77B6D8CD-05A0-4B1F-9E14-B895203BF40C";

        /// <summary>
        /// The target entity is a product which is manufactured by the source
        /// </summary>
        public const string ManufacturedProduct = "6780DF3B-AFBD-44A3-8627-CBB3DC2F02F6";

        /// <summary>
        /// The maternal aunt
        /// </summary>
        public const string MaternalAunt = "96EA355D-0C68-481F-8B6F-1B00A101AB8F";

        /// <summary>
        /// The maternal cousin
        /// </summary>
        public const string MaternalCousin = "D874CDE5-7D76-4F1D-97E6-DB7E82BAC958";

        /// <summary>
        /// The maternal grandfather
        /// </summary>
        public const string MaternalGrandfather = "360F6A77-FDB5-4FB6-B223-3CD1047FD08E";

        /// <summary>
        /// The maternal grandmother
        /// </summary>
        public const string MaternalGrandmother = "EA13832B-2E38-4BB6-B55D-AE749CCABA95";

        /// <summary>
        /// The maternal grandparent
        /// </summary>
        public const string MaternalGrandparent = "66E0DBD1-9065-4AF8-808D-89EDD302F264";

        /// <summary>
        /// The maternal greatgrandfather
        /// </summary>
        public const string MaternalGreatgrandfather = "ABE6D0D1-4E37-4B7C-9ACC-EEDB2C36F9CD";

        /// <summary>
        /// The maternal greatgrandmother
        /// </summary>
        public const string MaternalGreatgrandmother = "FE4F72E6-84F8-4276-AE64-2EF1F2FF406F";

        /// <summary>
        /// The maternal greatgrandparent
        /// </summary>
        public const string MaternalGreatgrandparent = "59BC87D3-1618-4F14-81D2-71072C1F37E9";

        /// <summary>
        /// The maternal uncle
        /// </summary>
        public const string MaternalUncle = "4E299C46-F06F-4EFC-B3C0-B7B659A120F2";

        /// <summary>
        /// The military person
        /// </summary>
        public const string MilitaryPerson = "1BCFB08D-C6FA-41DD-98BF-06336A33A3B7";

        /// <summary>
        /// The target is the mother of the source
        /// </summary>
        public const string Mother = "29FF64E5-B564-411A-92C7-6818C02A9E48";

        /// <summary>
        /// The motherinlaw
        /// </summary>
        public const string Motherinlaw = "F941988A-1C55-4408-AB57-E9ED35B2A24D";

        /// <summary>
        /// The target is a named insured person on the source policy
        /// </summary>
        public const string NamedInsured = "3D907F37-085C-4C26-B59B-62E40621DAFD";

        /// <summary>
        /// The natural brother
        /// </summary>
        public const string NaturalBrother = "DAF11EB1-FCC2-4521-A1C0-DAEBAF0A923A";

        /// <summary>
        /// The natural child
        /// </summary>
        public const string NaturalChild = "80097E75-A232-4A9F-878F-7E60EC70F921";

        /// <summary>
        /// The natural daughter
        /// </summary>
        public const string NaturalDaughter = "6A181A3C-7241-4325-B011-630D3CA6DC4A";

        /// <summary>
        /// The natural father
        /// </summary>
        public const string NaturalFather = "233D890B-04EF-4365-99AD-26CB4E1F75F3";

        /// <summary>
        /// The target is the natural father of fetus of the identified fetus (source) or pregnant entity (source)
        /// </summary>
        public const string NaturalFatherOfFetus = "8E88DEBC-D175-46F3-9B48-106F9C151CD2";

        /// <summary>
        /// The natural mother
        /// </summary>
        public const string NaturalMother = "059D689A-2392-4FFB-B6AE-682C9DED8DA2";

        /// <summary>
        /// The natural parent
        /// </summary>
        public const string NaturalParent = "E6851B39-A771-4A5E-8AA8-9BA140B3DCA3";

        /// <summary>
        /// The natural sibling
        /// </summary>
        public const string NaturalSibling = "0B89FB65-CA8E-4A4D-9D25-0BAE3F4D7A59";

        /// <summary>
        /// The natural sister
        /// </summary>
        public const string NaturalSister = "8EA21D7D-6EE9-449B-A1DC-C4AA0FF7F5B9";

        /// <summary>
        /// The natural son
        /// </summary>
        public const string NaturalSon = "9F17D4CF-A67F-4AC6-8C50-718AF6E264EE";

        /// <summary>
        /// The nephew
        /// </summary>
        public const string Nephew = "5C5AF1D2-0E6D-458F-9574-3AD61C393A90";

        /// <summary>
        /// The target is the next of kin for the source
        /// </summary>
        public const string NextOfKin = "1EE4E74F-542D-4544-96F6-266A6247F274";

        /// <summary>
        /// The niece
        /// </summary>
        public const string Niece = "0A50962A-60B4-44D8-A7F6-1EB2AA5967CC";

        /// <summary>
        /// The niece nephew
        /// </summary>
        public const string NieceNephew = "A907E4D8-D823-478F-9C5A-6FACAE6B4B5B";

        /// <summary>
        /// The target is a notary public acting within the source entity
        /// </summary>
        public const string NotaryPublic = "F1EF6C46-05EB-4482-BAEB-EAF0A8E5FFEF";

        /// <summary>
        /// The target entity is owned by the source entity
        /// </summary>
        public const string OwnedEntity = "117DA15C-0864-4F00-A987-9B9854CBA44E";

        /// <summary>
        /// The target entity is the parent of the source entity
        /// </summary>
        public const string Parent = "BFCBB345-86DB-43BA-B47E-E7411276AC7C";

        /// <summary>
        /// The parent inlaw
        /// </summary>
        public const string ParentInlaw = "5E2B0AFE-724E-41CD-9BE2-9030646F2529";

        /// <summary>
        /// The target entity is a part of the source entity (source is comprised of parts)
        /// </summary>
        public const string Part = "B2FEB552-8EAF-45FE-A397-F789D6F4728A";

        /// <summary>
        /// The paternal aunt
        /// </summary>
        public const string PaternalAunt = "6A1E9E8B-D0C3-44F0-9906-A6458685E269";

        /// <summary>
        /// The paternal cousin
        /// </summary>
        public const string PaternalCousin = "60AFFE56-126D-43EE-9FDE-5F117E41C7A8";

        /// <summary>
        /// The paternal grandfather
        /// </summary>
        public const string PaternalGrandfather = "2FD5C939-C508-4250-8EFB-13B772E56B7F";

        /// <summary>
        /// The paternal grandmother
        /// </summary>
        public const string PaternalGrandmother = "BFDB07DB-9721-4EC3-94E1-4BD9F0D6985C";

        /// <summary>
        /// The paternal grandparent
        /// </summary>
        public const string PaternalGrandparent = "A3D362A4-4931-4BEF-AF18-AC59DD092981";

        /// <summary>
        /// The paternal greatgrandfather
        /// </summary>
        public const string PaternalGreatgrandfather = "0AEEC758-C20F-43E4-9789-8C44629F5941";

        /// <summary>
        /// The paternal greatgrandmother
        /// </summary>
        public const string PaternalGreatgrandmother = "0FCBA203-1238-4001-BEB7-19A667506ADE";

        /// <summary>
        /// The paternal greatgrandparent
        /// </summary>
        public const string PaternalGreatgrandparent = "08A98950-3391-4A66-A1C8-421C6FD82911";

        /// <summary>
        /// The paternal uncle
        /// </summary>
        public const string PaternalUncle = "853C85DE-4817-4328-A121-6A3BDAFBF82E";

        /// <summary>
        /// The target is a patient of the source entity
        /// </summary>
        public const string Patient = "BACD9C6F-3FA9-481E-9636-37457962804D";

        /// <summary>
        /// The targert is a payee of the source entity
        /// </summary>
        public const string Payee = "734551E1-2960-4A68-93A2-B277DB072A43";

        /// <summary>
        /// The target possesses a personal relationship with the source entity
        /// </summary>
        public const string PersonalRelationship = "ABFD3FE8-9526-48FB-B366-35BACA9BD170";

        /// <summary>
        /// The target entity represents the place of death of the source entity
        /// </summary>
        public const string PlaceOfDeath = "9BBE0CFE-FAAB-4DC9-A28F-C001E3E95E6E";

        /// <summary>
        /// The target entity represents the policy holder of the source policy
        /// </summary>
        public const string PolicyHolder = "CEC017EF-4E49-41AF-8596-ABAD1A91C9D0";

        /// <summary>
        /// The target is an entity which is eligible for funding or participation within a program
        /// </summary>
        public const string ProgramEligible = "CBE2A00C-E1D5-44E9-AAE3-D7D03E3C2EFA";

        /// <summary>
        /// The target represents a qualified version of the source entity
        /// </summary>
        public const string QualifiedEntity = "6521DD09-334B-4FBF-9C89-1AD5A804326C";

        /// <summary>
        /// The target represents a regulated version of the source product or represents a product which is regulated within the source jurisdiction
        /// </summary>
        public const string RegulatedProduct = "20E98D17-E24D-4C64-B09E-521A177CCD05";

        /// <summary>
        /// The target represents a research subject of the source study
        /// </summary>
        public const string ResearchSubject = "EF597FFE-D965-4398-B55A-650530EBB997";

        /// <summary>
        /// The target represents a material which is a retailed version of the source or is sold at the particular source
        /// </summary>
        public const string RetailedMaterial = "703DF8F4-B124-44C5-9506-1AB74DDFD91D";

        /// <summary>
        /// The roomate
        /// </summary>
        public const string Roomate = "BBFAC1ED-5464-4100-93C3-8685B052A2CF";

        /// <summary>
        /// The target represents a service delivery location for the source entity
        /// </summary>
        public const string ServiceDeliveryLocation = "FF34DFA7-C6D3-4F8B-BC9F-14BCDC13BA6C";

        /// <summary>
        /// The sibling
        /// </summary>
        public const string Sibling = "685EB506-6B97-41C1-B201-B6B932A3F3AA";

        /// <summary>
        /// The sibling inlaw
        /// </summary>
        public const string SiblingInlaw = "FD892CF8-DB4F-4E4E-A13B-4EB3BDDE5BE5";

        /// <summary>
        /// The significant other
        /// </summary>
        public const string SignificantOther = "2EAB5298-BC83-492C-9004-ED3499246AFE";

        /// <summary>
        /// The target has signing authority or is an officer of the source
        /// </summary>
        public const string SigningAuthorityOrOfficer = "757F98DF-14E0-446A-BD50-BB0AFFB34F09";

        /// <summary>
        /// The sister
        /// </summary>
        public const string Sister = "CD1E8904-31DC-4374-902D-C91F1DE23C46";

        /// <summary>
        /// The sisterinlaw
        /// </summary>
        public const string Sisterinlaw = "DCAE9718-AB81-4737-B071-36CF1175804D";

        /// <summary>
        /// The son
        /// </summary>
        public const string Son = "F115C204-8485-4CF3-8815-3C6738465E30";

        /// <summary>
        /// The son inlaw
        /// </summary>
        public const string SonInlaw = "34F7BC11-2288-471A-AF38-553AE6B8410C";

        /// <summary>
        /// The target represents a specimen collected from the source
        /// </summary>
        public const string Specimen = "BCE17B21-05B2-4F02-BF7A-C6D3561AA948";

        /// <summary>
        /// The spouse
        /// </summary>
        public const string Spouse = "89BDC57B-D85C-4E85-94E8-C17049540A0D";

        /// <summary>
        /// The stepbrother
        /// </summary>
        public const string Stepbrother = "5951097B-1A13-4BCE-BBF2-9ABF52F98DC8";

        /// <summary>
        /// The step child
        /// </summary>
        public const string StepChild = "4CDEF917-4FB0-4CDF-B44D-B73486C41845";

        /// <summary>
        /// The stepdaughter
        /// </summary>
        public const string Stepdaughter = "F71E193A-0562-46E9-99DD-437D23663EC3";

        /// <summary>
        /// The stepfather
        /// </summary>
        public const string Stepfather = "BB437E4D-7472-48C1-A6E7-576545A782FA";

        /// <summary>
        /// The stepmother
        /// </summary>
        public const string Stepmother = "5A0539CC-093B-448E-AEC6-0D529ED0087F";

        /// <summary>
        /// The step parent
        /// </summary>
        public const string StepParent = "F172EEE7-7F4B-4022-81D0-76393A1200AE";

        /// <summary>
        /// The step sibling
        /// </summary>
        public const string StepSibling = "7E6BC25D-5DEA-4645-AF3D-AA854B7B6F2F";

        /// <summary>
        /// The stepsister
        /// </summary>
        public const string Stepsister = "CB73D085-026C-4BC7-A1DE-356BFD636246";

        /// <summary>
        /// The stepson
        /// </summary>
        public const string Stepson = "CFA978F4-140C-430D-82F8-1E6F2D74F48D";

        /// <summary>
        /// The student
        /// </summary>
        public const string Student = "0C157566-D1E9-4976-8542-473CAA9BA2A4";

        /// <summary>
        /// The target is a subscriber of the source, meaning the target should receive updates whenever the source changes
        /// </summary>
        public const string Subscriber = "F31A2A5B-CE13-47E1-A0FB-D704F31547DB";

        /// <summary>
        /// The target represents another territory where the source has authority
        /// </summary>
        public const string TerritoryOfAuthority = "C6B92576-1D62-4896-8799-6F931F8AB607";

        /// <summary>
        /// The target represents the theraputic agent of the source
        /// </summary>
        public const string TherapeuticAgent = "D6657FDB-4EF3-4131-AF79-14E01A21FACA";

        /// <summary>
        /// The uncle
        /// </summary>
        public const string Uncle = "CDD99260-107C-4A4E-ACAF-D7C9C7E90FDD";

        /// <summary>
        /// The underwriter
        /// </summary>
        public const string Underwriter = "A8FCD83F-808B-494B-8A1C-EC2C6DBC3DFA";

        /// <summary>
        /// The target represents an entity that is consumed whenever the source is consumed
        /// </summary>
        public const string UsedEntity = "08FFF7D9-BAC7-417B-B026-C9BEE52F4A37";

        /// <summary>
        /// The target represents a product which is warranted by the source
        /// </summary>
        public const string WarrantedProduct = "639B4B8F-AFD3-4963-9E79-EF0D3928796A";

        /// <summary>
        /// The wife
        /// </summary>
        public const string Wife = "A3FF423E-81D5-4571-8EDF-03C295189A23";

        /// <summary>
        /// The source replaces the target (note: this is one relationship where the role relationship is reveresed)
        /// </summary>
        public const string Replaces = "D1578637-E1CB-415E-B319-4011DA033813";

        /// <summary>
        /// The target entity represents an instance of the scoper entity
        /// </summary>
        public const string Instance = "AC45A740-B0C7-4425-84D8-B3F8A41FEF9F";

        /// <summary>
        /// Relates the target entity to a source location
        /// </summary>
        public const string LocatedEntity = "4F6273D3-8223-4E18-8596-C718AD029DEB";

        /// <summary>
        /// Duplicate entity
        /// </summary>
        public const string Duplicate = "2BBF068B-9121-4081-BF3C-AB62C01362EE";

        /// <summary>
        /// Entity is scoped by target
        /// </summary>
        public const string Scoper = "FCD37959-5BC2-48DB-BBB5-36AFD9EDF19A";

        /// <summary>
        /// Entity is just a link to another entity
        /// </summary>
        public const string EquivalentEntity = "395F4EDF-5D5D-4950-9F5E-F827F72E4B32";

        /// <summary>
        /// The source entity has an ingredient represented by the target
        /// </summary>
        public const string HasIngredient = "1F7163DF-BD86-436D-A14C-B225CDC630C5";

        /// <summary>
        /// The source entity is comprised of the target. Note that this differs from PART in that content can be separated, parts cannot be separated
        /// </summary>
        public const string HasContent = "9B127E8C-3703-42A9-8A7A-BBAFC6AB2C00";

        /// <summary>
        /// The source entity is a specialization of the target (i.e. the target is a more general entity kind than the source)
        /// </summary>
        public const string HasGenerialization = "BF9F929A-E7EB-4E5B-B82E-CF44384F0A3B";

        /// <summary>
        /// The source entity is comprised of the target as a part (example: DTP vaccine kind has part Diptheria vaccine kind, Tetanus vaccine kind, and Pertussis vaccine kind)
        /// </summary>
        public const string HasPart = "2220EF3F-B8D9-43A4-9BAE-A2906E3C0803";
    }

    /// <summary>
    /// Base entity relationship type keys
    /// </summary>
    public static class EntityRelationshipTypeKeys
    {
        /// <summary>
        /// The source entity gives access to the target entity
        /// </summary>
        public static readonly Guid Access = Guid.Parse(EntityRelationshipTypeKeyStrings.Access);

        /// <summary>
        /// Active ingredient, where not the ingredient substance (player), but itaTMs active moiety is the "basis of strength"
        /// </summary>
        public static readonly Guid ActiveMoiety = Guid.Parse(EntityRelationshipTypeKeyStrings.ActiveMoiety);

        /// <summary>
        /// The source represents a meterial that is an administerable form of the target
        /// </summary>
        public static readonly Guid AdministerableMaterial = Guid.Parse(EntityRelationshipTypeKeyStrings.AdministerableMaterial);

        /// <summary>
        /// The source is an adopted child of the target
        /// </summary>
        public static readonly Guid AdoptedChild = Guid.Parse(EntityRelationshipTypeKeyStrings.AdoptedChild);

        /// <summary>
        /// The source is an adopted daughter of the target
        /// </summary>
        public static readonly Guid AdoptedDaughter = Guid.Parse(EntityRelationshipTypeKeyStrings.AdoptedDaughter);

        /// <summary>
        /// The source is an adopted son of the target
        /// </summary>
        public static readonly Guid AdoptedSon = Guid.Parse(EntityRelationshipTypeKeyStrings.AdoptedSon);

        /// <summary>
        /// The target has a business/professional relationship with the source.
        /// </summary>
        public static readonly Guid Affiliate = Guid.Parse(EntityRelationshipTypeKeyStrings.Affiliate);

        /// <summary>
        /// The target is an agent or authorized to act on behalf of the source
        /// </summary>
        public static readonly Guid Agent = Guid.Parse(EntityRelationshipTypeKeyStrings.Agent);

        /// <summary>
        /// The target is a portion of the original source
        /// </summary>
        public static readonly Guid Aliquot = Guid.Parse(EntityRelationshipTypeKeyStrings.Aliquot);

        /// <summary>
        /// The target is an entity acting under role or assignment of the source 
        /// </summary>
        public static readonly Guid AssignedEntity = Guid.Parse(EntityRelationshipTypeKeyStrings.AssignedEntity);

        /// <summary>
        /// The target is the aunt of the source
        /// </summary>
        public static readonly Guid Aunt = Guid.Parse(EntityRelationshipTypeKeyStrings.Aunt);

        /// <summary>
        /// The target is the birthplace of the source
        /// </summary>
        public static readonly Guid Birthplace = Guid.Parse(EntityRelationshipTypeKeyStrings.Birthplace);

        /// <summary>
        /// The target is the brother of the source
        /// </summary>
        public static readonly Guid Brother = Guid.Parse(EntityRelationshipTypeKeyStrings.Brother);

        /// <summary>
        /// The target is the brotherinlaw of the source
        /// </summary>
        public static readonly Guid Brotherinlaw = Guid.Parse(EntityRelationshipTypeKeyStrings.Brotherinlaw);

        /// <summary>
        /// The target is the caregiver of the source
        /// </summary>
        public static readonly Guid Caregiver = Guid.Parse(EntityRelationshipTypeKeyStrings.Caregiver);

        /// <summary>
        /// The the target represents a case subject of the source entity (such as a study)
        /// </summary>
        public static readonly Guid CaseSubject = Guid.Parse(EntityRelationshipTypeKeyStrings.CaseSubject);

        /// <summary>
        /// The the target is a child of the child source
        /// </summary>
        public static readonly Guid Child = Guid.Parse(EntityRelationshipTypeKeyStrings.Child);

        /// <summary>
        /// The child inlaw
        /// </summary>
        public static readonly Guid ChildInlaw = Guid.Parse(EntityRelationshipTypeKeyStrings.ChildInlaw);

        /// <summary>
        /// The target is a citizen of the source
        /// </summary>
        public static readonly Guid Citizen = Guid.Parse(EntityRelationshipTypeKeyStrings.Citizen);

        /// <summary>
        /// The target is a claimant  or is making a claim in a policy (source)
        /// </summary>
        public static readonly Guid Claimant = Guid.Parse(EntityRelationshipTypeKeyStrings.Claimant);

        /// <summary>
        /// The clinical research investigator
        /// </summary>
        public static readonly Guid ClinicalResearchInvestigator = Guid.Parse(EntityRelationshipTypeKeyStrings.ClinicalResearchInvestigator);

        /// <summary>
        /// The clinical research sponsor
        /// </summary>
        public static readonly Guid ClinicalResearchSponsor = Guid.Parse(EntityRelationshipTypeKeyStrings.ClinicalResearchSponsor);

        /// <summary>
        /// The commissioning party
        /// </summary>
        public static readonly Guid CommissioningParty = Guid.Parse(EntityRelationshipTypeKeyStrings.CommissioningParty);

        /// <summary>
        /// Community location which is used to provide services within holder
        /// </summary>
        public static readonly Guid CommunityServiceDeliveryLocation = Guid.Parse(EntityRelationshipTypeKeyStrings.CommunityServiceDeliveryLocation);

        /// <summary>
        /// The target represents a contact of the source
        /// </summary>
        public static readonly Guid Contact = Guid.Parse(EntityRelationshipTypeKeyStrings.Contact);

        /// <summary>
        /// The cousin
        /// </summary>
        public static readonly Guid Cousin = Guid.Parse(EntityRelationshipTypeKeyStrings.Cousin);

        /// <summary>
        /// The target represents a coverage sponsor of the source
        /// </summary>
        public static readonly Guid CoverageSponsor = Guid.Parse(EntityRelationshipTypeKeyStrings.CoverageSponsor);

        /// <summary>
        /// The target is a covered party of a source (insurance policy)
        /// </summary>
        public static readonly Guid CoveredParty = Guid.Parse(EntityRelationshipTypeKeyStrings.CoveredParty);

        /// <summary>
        /// The daughter
        /// </summary>
        public static readonly Guid Daughter = Guid.Parse(EntityRelationshipTypeKeyStrings.Daughter);

        /// <summary>
        /// The daughter inlaw
        /// </summary>
        public static readonly Guid DaughterInlaw = Guid.Parse(EntityRelationshipTypeKeyStrings.DaughterInlaw);

        /// <summary>
        /// The target is the dedicated service delivery location for the source
        /// </summary>
        public static readonly Guid DedicatedServiceDeliveryLocation = Guid.Parse(EntityRelationshipTypeKeyStrings.DedicatedServiceDeliveryLocation);

        /// <summary>
        /// The target is a dependent of the source
        /// </summary>
        public static readonly Guid Dependent = Guid.Parse(EntityRelationshipTypeKeyStrings.Dependent);

        /// <summary>
        /// The target is a distributed or shippable material of the source
        /// </summary>
        public static readonly Guid DistributedMaterial = Guid.Parse(EntityRelationshipTypeKeyStrings.DistributedMaterial);

        /// <summary>
        /// The domestic partner
        /// </summary>
        public static readonly Guid DomesticPartner = Guid.Parse(EntityRelationshipTypeKeyStrings.DomesticPartner);

        /// <summary>
        /// The target is an emergency contact for the source
        /// </summary>
        public static readonly Guid EmergencyContact = Guid.Parse(EntityRelationshipTypeKeyStrings.EmergencyContact);

        /// <summary>
        /// The the target is an employee of the source
        /// </summary>
        public static readonly Guid Employee = Guid.Parse(EntityRelationshipTypeKeyStrings.Employee);

        /// <summary>
        /// The target represents a substance which is exposed when the source is exposed
        /// </summary>
        public static readonly Guid ExposedEntity = Guid.Parse(EntityRelationshipTypeKeyStrings.ExposedEntity);

        /// <summary>
        /// The family member
        /// </summary>
        public static readonly Guid FamilyMember = Guid.Parse(EntityRelationshipTypeKeyStrings.FamilyMember);

        /// <summary>
        /// The father
        /// </summary>
        public static readonly Guid Father = Guid.Parse(EntityRelationshipTypeKeyStrings.Father);

        /// <summary>
        /// The fatherinlaw
        /// </summary>
        public static readonly Guid Fatherinlaw = Guid.Parse(EntityRelationshipTypeKeyStrings.Fatherinlaw);

        /// <summary>
        /// The foster child
        /// </summary>
        public static readonly Guid FosterChild = Guid.Parse(EntityRelationshipTypeKeyStrings.FosterChild);

        /// <summary>
        /// The foster daughter
        /// </summary>
        public static readonly Guid FosterDaughter = Guid.Parse(EntityRelationshipTypeKeyStrings.FosterDaughter);

        /// <summary>
        /// The foster son
        /// </summary>
        public static readonly Guid FosterSon = Guid.Parse(EntityRelationshipTypeKeyStrings.FosterSon);

        /// <summary>
        /// The grandchild
        /// </summary>
        public static readonly Guid Grandchild = Guid.Parse(EntityRelationshipTypeKeyStrings.Grandchild);

        /// <summary>
        /// The granddaughter
        /// </summary>
        public static readonly Guid Granddaughter = Guid.Parse(EntityRelationshipTypeKeyStrings.Granddaughter);

        /// <summary>
        /// The grandfather
        /// </summary>
        public static readonly Guid Grandfather = Guid.Parse(EntityRelationshipTypeKeyStrings.Grandfather);

        /// <summary>
        /// The grandmother
        /// </summary>
        public static readonly Guid Grandmother = Guid.Parse(EntityRelationshipTypeKeyStrings.Grandmother);

        /// <summary>
        /// The grandparent
        /// </summary>
        public static readonly Guid Grandparent = Guid.Parse(EntityRelationshipTypeKeyStrings.Grandparent);

        /// <summary>
        /// The grandson
        /// </summary>
        public static readonly Guid Grandson = Guid.Parse(EntityRelationshipTypeKeyStrings.Grandson);

        /// <summary>
        /// The great grandfather
        /// </summary>
        public static readonly Guid GreatGrandfather = Guid.Parse(EntityRelationshipTypeKeyStrings.GreatGrandfather);

        /// <summary>
        /// The great grandmother
        /// </summary>
        public static readonly Guid GreatGrandmother = Guid.Parse(EntityRelationshipTypeKeyStrings.GreatGrandmother);

        /// <summary>
        /// The great grandparent
        /// </summary>
        public static readonly Guid GreatGrandparent = Guid.Parse(EntityRelationshipTypeKeyStrings.GreatGrandparent);

        /// <summary>
        /// The guarantor
        /// </summary>
        public static readonly Guid Guarantor = Guid.Parse(EntityRelationshipTypeKeyStrings.Guarantor);

        /// <summary>
        /// The guard
        /// </summary>
        public static readonly Guid GUARD = Guid.Parse(EntityRelationshipTypeKeyStrings.GUARD);

        /// <summary>
        /// The target is a guardian of the source
        /// </summary>
        public static readonly Guid Guardian = Guid.Parse(EntityRelationshipTypeKeyStrings.Guardian);

        /// <summary>
        /// The halfbrother
        /// </summary>
        public static readonly Guid Halfbrother = Guid.Parse(EntityRelationshipTypeKeyStrings.Halfbrother);

        /// <summary>
        /// The halfsibling
        /// </summary>
        public static readonly Guid Halfsibling = Guid.Parse(EntityRelationshipTypeKeyStrings.Halfsibling);

        /// <summary>
        /// The halfsister
        /// </summary>
        public static readonly Guid Halfsister = Guid.Parse(EntityRelationshipTypeKeyStrings.Halfsister);

        /// <summary>
        /// The target is a healthcare provider for the source
        /// </summary>
        public static readonly Guid HealthcareProvider = Guid.Parse(EntityRelationshipTypeKeyStrings.HealthcareProvider);

        /// <summary>
        /// The target represents a health chart belonging to the source
        /// </summary>
        public static readonly Guid HealthChart = Guid.Parse(EntityRelationshipTypeKeyStrings.HealthChart);

        /// <summary>
        /// The source holds the specified quantity of the target entity (the target entity is held by the source)
        /// </summary>
        public static readonly Guid HeldEntity = Guid.Parse(EntityRelationshipTypeKeyStrings.HeldEntity);

        /// <summary>
        /// The husband
        /// </summary>
        public static readonly Guid Husband = Guid.Parse(EntityRelationshipTypeKeyStrings.Husband);

        /// <summary>
        /// The target represents an entity for purposes of identification of the source
        /// </summary>
        public static readonly Guid IdentifiedEntity = Guid.Parse(EntityRelationshipTypeKeyStrings.IdentifiedEntity);

        /// <summary>
        /// The target represents an incidental service delivery location related to the source entity
        /// </summary>
        public static readonly Guid IncidentalServiceDeliveryLocation = Guid.Parse(EntityRelationshipTypeKeyStrings.IncidentalServiceDeliveryLocation);

        /// <summary>
        /// The target represents an individual instance of the source
        /// </summary>
        public static readonly Guid Individual = Guid.Parse(EntityRelationshipTypeKeyStrings.Individual);

        /// <summary>
        /// The investigation subject
        /// </summary>
        public static readonly Guid InvestigationSubject = Guid.Parse(EntityRelationshipTypeKeyStrings.InvestigationSubject);

        /// <summary>
        /// The target is the payor of an invoice for the source
        /// </summary>
        public static readonly Guid InvoicePayor = Guid.Parse(EntityRelationshipTypeKeyStrings.InvoicePayor);

        /// <summary>
        /// The isolate
        /// </summary>
        public static readonly Guid Isolate = Guid.Parse(EntityRelationshipTypeKeyStrings.Isolate);

        /// <summary>
        /// The target represents an entity licensed to perform or use the source
        /// </summary>
        public static readonly Guid LicensedEntity = Guid.Parse(EntityRelationshipTypeKeyStrings.LicensedEntity);

        /// <summary>
        /// The target entity is maintained by the source entity
        /// </summary>
        public static readonly Guid MaintainedEntity = Guid.Parse(EntityRelationshipTypeKeyStrings.MaintainedEntity);

        /// <summary>
        /// The target entity is a product which is manufactured by the source
        /// </summary>
        public static readonly Guid ManufacturedProduct = Guid.Parse(EntityRelationshipTypeKeyStrings.ManufacturedProduct);

        /// <summary>
        /// The maternal aunt
        /// </summary>
        public static readonly Guid MaternalAunt = Guid.Parse(EntityRelationshipTypeKeyStrings.MaternalAunt);

        /// <summary>
        /// The maternal cousin
        /// </summary>
        public static readonly Guid MaternalCousin = Guid.Parse(EntityRelationshipTypeKeyStrings.MaternalCousin);

        /// <summary>
        /// The maternal grandfather
        /// </summary>
        public static readonly Guid MaternalGrandfather = Guid.Parse(EntityRelationshipTypeKeyStrings.MaternalGrandfather);

        /// <summary>
        /// The maternal grandmother
        /// </summary>
        public static readonly Guid MaternalGrandmother = Guid.Parse(EntityRelationshipTypeKeyStrings.MaternalGrandmother);

        /// <summary>
        /// The maternal grandparent
        /// </summary>
        public static readonly Guid MaternalGrandparent = Guid.Parse(EntityRelationshipTypeKeyStrings.MaternalGrandparent);

        /// <summary>
        /// The maternal greatgrandfather
        /// </summary>
        public static readonly Guid MaternalGreatgrandfather = Guid.Parse(EntityRelationshipTypeKeyStrings.MaternalGreatgrandfather);

        /// <summary>
        /// The maternal greatgrandmother
        /// </summary>
        public static readonly Guid MaternalGreatgrandmother = Guid.Parse(EntityRelationshipTypeKeyStrings.MaternalGreatgrandmother);

        /// <summary>
        /// The maternal greatgrandparent
        /// </summary>
        public static readonly Guid MaternalGreatgrandparent = Guid.Parse(EntityRelationshipTypeKeyStrings.MaternalGreatgrandparent);

        /// <summary>
        /// The maternal uncle
        /// </summary>
        public static readonly Guid MaternalUncle = Guid.Parse(EntityRelationshipTypeKeyStrings.MaternalUncle);

        /// <summary>
        /// The military person
        /// </summary>
        public static readonly Guid MilitaryPerson = Guid.Parse(EntityRelationshipTypeKeyStrings.MilitaryPerson);

        /// <summary>
        /// The target is the mother of the source
        /// </summary>
        public static readonly Guid Mother = Guid.Parse(EntityRelationshipTypeKeyStrings.Mother);

        /// <summary>
        /// The motherinlaw
        /// </summary>
        public static readonly Guid Motherinlaw = Guid.Parse(EntityRelationshipTypeKeyStrings.Motherinlaw);

        /// <summary>
        /// The target is a named insured person on the source policy
        /// </summary>
        public static readonly Guid NamedInsured = Guid.Parse(EntityRelationshipTypeKeyStrings.NamedInsured);

        /// <summary>
        /// The natural brother
        /// </summary>
        public static readonly Guid NaturalBrother = Guid.Parse(EntityRelationshipTypeKeyStrings.NaturalBrother);

        /// <summary>
        /// The natural child
        /// </summary>
        public static readonly Guid NaturalChild = Guid.Parse(EntityRelationshipTypeKeyStrings.NaturalChild);

        /// <summary>
        /// The natural daughter
        /// </summary>
        public static readonly Guid NaturalDaughter = Guid.Parse(EntityRelationshipTypeKeyStrings.NaturalDaughter);

        /// <summary>
        /// The natural father
        /// </summary>
        public static readonly Guid NaturalFather = Guid.Parse(EntityRelationshipTypeKeyStrings.NaturalFather);

        /// <summary>
        /// The target is the natural father of fetus of the identified fetus (source) or pregnant entity (source)
        /// </summary>
        public static readonly Guid NaturalFatherOfFetus = Guid.Parse(EntityRelationshipTypeKeyStrings.NaturalFatherOfFetus);

        /// <summary>
        /// The natural mother
        /// </summary>
        public static readonly Guid NaturalMother = Guid.Parse(EntityRelationshipTypeKeyStrings.NaturalMother);

        /// <summary>
        /// The natural parent
        /// </summary>
        public static readonly Guid NaturalParent = Guid.Parse(EntityRelationshipTypeKeyStrings.NaturalParent);

        /// <summary>
        /// The natural sibling
        /// </summary>
        public static readonly Guid NaturalSibling = Guid.Parse(EntityRelationshipTypeKeyStrings.NaturalSibling);

        /// <summary>
        /// The natural sister
        /// </summary>
        public static readonly Guid NaturalSister = Guid.Parse(EntityRelationshipTypeKeyStrings.NaturalSister);

        /// <summary>
        /// The natural son
        /// </summary>
        public static readonly Guid NaturalSon = Guid.Parse(EntityRelationshipTypeKeyStrings.NaturalSon);

        /// <summary>
        /// The nephew
        /// </summary>
        public static readonly Guid Nephew = Guid.Parse(EntityRelationshipTypeKeyStrings.Nephew);

        /// <summary>
        /// The target is the next of kin for the source
        /// </summary>
        public static readonly Guid NextOfKin = Guid.Parse(EntityRelationshipTypeKeyStrings.NextOfKin);

        /// <summary>
        /// The niece
        /// </summary>
        public static readonly Guid Niece = Guid.Parse(EntityRelationshipTypeKeyStrings.Niece);

        /// <summary>
        /// The niece nephew
        /// </summary>
        public static readonly Guid NieceNephew = Guid.Parse(EntityRelationshipTypeKeyStrings.NieceNephew);

        /// <summary>
        /// The target is a notary public acting within the source entity
        /// </summary>
        public static readonly Guid NotaryPublic = Guid.Parse(EntityRelationshipTypeKeyStrings.NotaryPublic);

        /// <summary>
        /// The target entity is owned by the source entity
        /// </summary>
        public static readonly Guid OwnedEntity = Guid.Parse(EntityRelationshipTypeKeyStrings.OwnedEntity);

        /// <summary>
        /// The target entity is the parent of the source entity
        /// </summary>
        public static readonly Guid Parent = Guid.Parse(EntityRelationshipTypeKeyStrings.Parent);

        /// <summary>
        /// The parent inlaw
        /// </summary>
        public static readonly Guid ParentInlaw = Guid.Parse(EntityRelationshipTypeKeyStrings.ParentInlaw);

        /// <summary>
        /// The target entity is a part of the source entity (source is comprised of parts)
        /// </summary>
        public static readonly Guid Part = Guid.Parse(EntityRelationshipTypeKeyStrings.Part);

        /// <summary>
        /// The paternal aunt
        /// </summary>
        public static readonly Guid PaternalAunt = Guid.Parse(EntityRelationshipTypeKeyStrings.PaternalAunt);

        /// <summary>
        /// The paternal cousin
        /// </summary>
        public static readonly Guid PaternalCousin = Guid.Parse(EntityRelationshipTypeKeyStrings.PaternalCousin);

        /// <summary>
        /// The paternal grandfather
        /// </summary>
        public static readonly Guid PaternalGrandfather = Guid.Parse(EntityRelationshipTypeKeyStrings.PaternalGrandfather);

        /// <summary>
        /// The paternal grandmother
        /// </summary>
        public static readonly Guid PaternalGrandmother = Guid.Parse(EntityRelationshipTypeKeyStrings.PaternalGrandmother);

        /// <summary>
        /// The paternal grandparent
        /// </summary>
        public static readonly Guid PaternalGrandparent = Guid.Parse(EntityRelationshipTypeKeyStrings.PaternalGrandparent);

        /// <summary>
        /// The paternal greatgrandfather
        /// </summary>
        public static readonly Guid PaternalGreatgrandfather = Guid.Parse(EntityRelationshipTypeKeyStrings.PaternalGreatgrandfather);

        /// <summary>
        /// The paternal greatgrandmother
        /// </summary>
        public static readonly Guid PaternalGreatgrandmother = Guid.Parse(EntityRelationshipTypeKeyStrings.PaternalGreatgrandmother);

        /// <summary>
        /// The paternal greatgrandparent
        /// </summary>
        public static readonly Guid PaternalGreatgrandparent = Guid.Parse(EntityRelationshipTypeKeyStrings.PaternalGreatgrandparent);

        /// <summary>
        /// The paternal uncle
        /// </summary>
        public static readonly Guid PaternalUncle = Guid.Parse(EntityRelationshipTypeKeyStrings.PaternalUncle);

        /// <summary>
        /// The target is a patient of the source entity
        /// </summary>
        public static readonly Guid Patient = Guid.Parse(EntityRelationshipTypeKeyStrings.Patient);

        /// <summary>
        /// The targert is a payee of the source entity
        /// </summary>
        public static readonly Guid Payee = Guid.Parse(EntityRelationshipTypeKeyStrings.Payee);

        /// <summary>
        /// The target possesses a personal relationship with the source entity
        /// </summary>
        public static readonly Guid PersonalRelationship = Guid.Parse(EntityRelationshipTypeKeyStrings.PersonalRelationship);

        /// <summary>
        /// The target entity represents the place of death of the source entity
        /// </summary>
        public static readonly Guid PlaceOfDeath = Guid.Parse(EntityRelationshipTypeKeyStrings.PlaceOfDeath);

        /// <summary>
        /// The target entity represents the policy holder of the source policy
        /// </summary>
        public static readonly Guid PolicyHolder = Guid.Parse(EntityRelationshipTypeKeyStrings.PolicyHolder);

        /// <summary>
        /// The target is an entity which is eligible for funding or participation within a program
        /// </summary>
        public static readonly Guid ProgramEligible = Guid.Parse(EntityRelationshipTypeKeyStrings.ProgramEligible);

        /// <summary>
        /// The target represents a qualified version of the source entity
        /// </summary>
        public static readonly Guid QualifiedEntity = Guid.Parse(EntityRelationshipTypeKeyStrings.QualifiedEntity);

        /// <summary>
        /// The target represents a regulated version of the source product or represents a product which is regulated within the source jurisdiction
        /// </summary>
        public static readonly Guid RegulatedProduct = Guid.Parse(EntityRelationshipTypeKeyStrings.RegulatedProduct);

        /// <summary>
        /// The target represents a research subject of the source study
        /// </summary>
        public static readonly Guid ResearchSubject = Guid.Parse(EntityRelationshipTypeKeyStrings.ResearchSubject);

        /// <summary>
        /// The target represents a material which is a retailed version of the source or is sold at the particular source
        /// </summary>
        public static readonly Guid RetailedMaterial = Guid.Parse(EntityRelationshipTypeKeyStrings.RetailedMaterial);

        /// <summary>
        /// The roomate
        /// </summary>
        public static readonly Guid Roomate = Guid.Parse(EntityRelationshipTypeKeyStrings.Roomate);

        /// <summary>
        /// The target represents a service delivery location for the source entity
        /// </summary>
        public static readonly Guid ServiceDeliveryLocation = Guid.Parse(EntityRelationshipTypeKeyStrings.ServiceDeliveryLocation);

        /// <summary>
        /// The sibling
        /// </summary>
        public static readonly Guid Sibling = Guid.Parse(EntityRelationshipTypeKeyStrings.Sibling);

        /// <summary>
        /// The sibling inlaw
        /// </summary>
        public static readonly Guid SiblingInlaw = Guid.Parse(EntityRelationshipTypeKeyStrings.SiblingInlaw);

        /// <summary>
        /// The significant other
        /// </summary>
        public static readonly Guid SignificantOther = Guid.Parse(EntityRelationshipTypeKeyStrings.SignificantOther);

        /// <summary>
        /// The target has signing authority or is an officer of the source
        /// </summary>
        public static readonly Guid SigningAuthorityOrOfficer = Guid.Parse(EntityRelationshipTypeKeyStrings.SigningAuthorityOrOfficer);

        /// <summary>
        /// The sister
        /// </summary>
        public static readonly Guid Sister = Guid.Parse(EntityRelationshipTypeKeyStrings.Sister);

        /// <summary>
        /// The sisterinlaw
        /// </summary>
        public static readonly Guid Sisterinlaw = Guid.Parse(EntityRelationshipTypeKeyStrings.Sisterinlaw);

        /// <summary>
        /// The son
        /// </summary>
        public static readonly Guid Son = Guid.Parse(EntityRelationshipTypeKeyStrings.Son);

        /// <summary>
        /// The son inlaw
        /// </summary>
        public static readonly Guid SonInlaw = Guid.Parse(EntityRelationshipTypeKeyStrings.SonInlaw);

        /// <summary>
        /// The target represents a specimen collected from the source
        /// </summary>
        public static readonly Guid Specimen = Guid.Parse(EntityRelationshipTypeKeyStrings.Specimen);

        /// <summary>
        /// The spouse
        /// </summary>
        public static readonly Guid Spouse = Guid.Parse(EntityRelationshipTypeKeyStrings.Spouse);

        /// <summary>
        /// The stepbrother
        /// </summary>
        public static readonly Guid Stepbrother = Guid.Parse(EntityRelationshipTypeKeyStrings.Stepbrother);

        /// <summary>
        /// The step child
        /// </summary>
        public static readonly Guid StepChild = Guid.Parse(EntityRelationshipTypeKeyStrings.StepChild);

        /// <summary>
        /// The stepdaughter
        /// </summary>
        public static readonly Guid Stepdaughter = Guid.Parse(EntityRelationshipTypeKeyStrings.Stepdaughter);

        /// <summary>
        /// The stepfather
        /// </summary>
        public static readonly Guid Stepfather = Guid.Parse(EntityRelationshipTypeKeyStrings.Stepfather);

        /// <summary>
        /// The stepmother
        /// </summary>
        public static readonly Guid Stepmother = Guid.Parse(EntityRelationshipTypeKeyStrings.Stepmother);

        /// <summary>
        /// The step parent
        /// </summary>
        public static readonly Guid StepParent = Guid.Parse(EntityRelationshipTypeKeyStrings.StepParent);

        /// <summary>
        /// The step sibling
        /// </summary>
        public static readonly Guid StepSibling = Guid.Parse(EntityRelationshipTypeKeyStrings.StepSibling);

        /// <summary>
        /// The stepsister
        /// </summary>
        public static readonly Guid Stepsister = Guid.Parse(EntityRelationshipTypeKeyStrings.Stepsister);

        /// <summary>
        /// The stepson
        /// </summary>
        public static readonly Guid Stepson = Guid.Parse(EntityRelationshipTypeKeyStrings.Stepson);

        /// <summary>
        /// The student
        /// </summary>
        public static readonly Guid Student = Guid.Parse(EntityRelationshipTypeKeyStrings.Student);

        /// <summary>
        /// The target is a subscriber of the source, meaning the target should receive updates whenever the source changes
        /// </summary>
        public static readonly Guid Subscriber = Guid.Parse(EntityRelationshipTypeKeyStrings.Subscriber);

        /// <summary>
        /// The target represents another territory where the source has authority
        /// </summary>
        public static readonly Guid TerritoryOfAuthority = Guid.Parse(EntityRelationshipTypeKeyStrings.TerritoryOfAuthority);

        /// <summary>
        /// The target represents the theraputic agent of the source
        /// </summary>
        public static readonly Guid TherapeuticAgent = Guid.Parse(EntityRelationshipTypeKeyStrings.TherapeuticAgent);

        /// <summary>
        /// The uncle
        /// </summary>
        public static readonly Guid Uncle = Guid.Parse(EntityRelationshipTypeKeyStrings.Uncle);

        /// <summary>
        /// The underwriter
        /// </summary>
        public static readonly Guid Underwriter = Guid.Parse(EntityRelationshipTypeKeyStrings.Underwriter);

        /// <summary>
        /// The target represents an entity that is consumed whenever the source is consumed
        /// </summary>
        public static readonly Guid UsedEntity = Guid.Parse(EntityRelationshipTypeKeyStrings.UsedEntity);

        /// <summary>
        /// The target represents a product which is warranted by the source
        /// </summary>
        public static readonly Guid WarrantedProduct = Guid.Parse(EntityRelationshipTypeKeyStrings.WarrantedProduct);

        /// <summary>
        /// The wife
        /// </summary>
        public static readonly Guid Wife = Guid.Parse(EntityRelationshipTypeKeyStrings.Wife);

        /// <summary>
        /// The source replaces the target (note: this is one relationship where the role relationship is reveresed)
        /// </summary>
        public static readonly Guid Replaces = Guid.Parse(EntityRelationshipTypeKeyStrings.Replaces);

        /// <summary>
        /// The target entity represents an instance of the scoper entity
        /// </summary>
        public static readonly Guid Instance = Guid.Parse(EntityRelationshipTypeKeyStrings.Instance);

        /// <summary>
        /// Relates the target entity to a source location
        /// </summary>
        public static readonly Guid LocatedEntity = Guid.Parse(EntityRelationshipTypeKeyStrings.LocatedEntity);

        /// <summary>
        /// Duplicate entity
        /// </summary>
        public static readonly Guid Duplicate = Guid.Parse(EntityRelationshipTypeKeyStrings.Duplicate);

        /// <summary>
        /// Duplicate entity
        /// </summary>
        public static readonly Guid Scoper = Guid.Parse(EntityRelationshipTypeKeyStrings.Scoper);

        /// <summary>
        /// Referenced entities
        /// </summary>
        public static readonly Guid EquivalentEntity = Guid.Parse(EntityRelationshipTypeKeyStrings.EquivalentEntity);

        /// <summary>
        /// The source entity has an ingredient represented by the target
        /// </summary>
        public static readonly Guid HasIngredient = Guid.Parse("1F7163DF-BD86-436D-A14C-B225CDC630C5");

        /// <summary>
        /// The source entity is comprised of the target. Note that this differs from PART in that content can be separated, parts cannot be separated
        /// </summary>
        public static readonly Guid HasContent = Guid.Parse("9B127E8C-3703-42A9-8A7A-BBAFC6AB2C00");

        /// <summary>
        /// The source entity is a specialization of the target (i.e. the target is a more general entity kind than the source)
        /// </summary>
        public static readonly Guid HasGenerialization = Guid.Parse("BF9F929A-E7EB-4E5B-B82E-CF44384F0A3B");

        /// <summary>
        /// The source entity is comprised of the target as a part (example: DTP vaccine kind has part Diptheria vaccine kind, Tetanus vaccine kind, and Pertussis vaccine kind)
        /// </summary>
        public static readonly Guid HasPart = Guid.Parse("2220EF3F-B8D9-43A4-9BAE-A2906E3C0803");
    }
}