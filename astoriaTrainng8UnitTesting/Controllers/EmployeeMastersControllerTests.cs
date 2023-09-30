using Microsoft.VisualStudio.TestTools.UnitTesting;
using astoriaTrainingAPI.Controllers;
using System;
using System.Collections.Generic;
using System.Text;
using astoriaTrainingAPI.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;

namespace astoriaTrainingAPI.Controllers.Tests
{
    [TestClass()]
    public class EmployeeMastersControllerTests
    {
        private readonly astoriaTraining80Context _context;
        public EmployeeMastersControllerTests()
        {
            var optionBuilder = new DbContextOptionsBuilder<astoriaTraining80Context>();
            optionBuilder.UseSqlServer("Server=ASTORIA-LT40;Database=astoriaTraining8.0;User ID=sa;Password=pass123;");
            _context = new astoriaTraining80Context(optionBuilder.Options);
        }

        #region Unit Test Methode Of GetEmployees API
        [TestMethod()]
        public void GetEmployees_Match_Total_Count_Test()
        {
            //Arrange 
            int expectedEmployeeCount = 24;

            //Action
            var objEmployeeMasterController = new EmployeeMastersController(_context);
            var apiResult = objEmployeeMasterController.GetEmployees();
            var resultList = apiResult.Result.Value as List<Employee>;
            int resultCount = resultList.Count;

            //Assert
            Assert.AreEqual(expectedEmployeeCount, resultCount);
        }


        [TestMethod()]
        public void GetEmployees_ReturnsNoConent_Test()
        {
            //Arrange 
            var noContentResult = typeof(NoContentResult);

            //Action
            var objEmployeeMasterController = new EmployeeMastersController(_context);
            var apiResult = objEmployeeMasterController.GetEmployees();
            var resultType = apiResult.Result.Result;

            //Assert
            Assert.AreEqual(resultType, noContentResult);
        }
        #endregion

        #region Unit Test Methode Of GetEmployeeMasterByID API

        [TestMethod()]
        public void GetEmployeeMaster_ValidInput_MatchResult_InputEmpKey_OutEmpKey_Test()
        {
            //Arrange 
            long empKeyInput = 19;

            //Action
            var objEmployeeMasterController = new EmployeeMastersController(_context);
            var result = objEmployeeMasterController.GetEmployeeMaster(empKeyInput);
            var resultObj = ((OkObjectResult)result.Result.Result).Value as EmployeeMaster;
            //Assert
            Assert.AreEqual(empKeyInput, resultObj.EmployeeKey);
        }

        [TestMethod()]
        public void GetEmployeeMaster_ValidInput_MatchResult_OkResult_Test()
        {
            //Arrange 
            long empKeyInput = 19;

            //Action
            var objEmployeeMasterController = new EmployeeMastersController(_context);
            var apiResult = objEmployeeMasterController.GetEmployeeMaster(empKeyInput);
            var resultObj = apiResult.Result.Result;
            //Assert
            Assert.IsInstanceOfType( resultObj, typeof(OkObjectResult));
        }

        [TestMethod()]
        public void GetEmployeeMaster_InValid_EmployeeKey_Input_MatchResult_Test()
        {
            //Arrange
            long empKeyInput = 1;

            //Action
            var objEmployeeMasterController = new EmployeeMastersController(_context);
            var result = objEmployeeMasterController.GetEmployeeMaster(empKeyInput);

            //Assert
            Assert.IsInstanceOfType(result.Result.Result, typeof(NotFoundResult));
        }
        #endregion

        #region Unit Test Methode Of GetAllCompany API
        [TestMethod()]
        public void GetAllCompany_TotalCount_Test()
        {
            //Arrange
            int expectTotalCompanyCount = 3;
            //Action
            var objEmployeeMasterController = new EmployeeMastersController(_context);
            var apiResult = objEmployeeMasterController.GetAllCompany();
            var resultList = apiResult.Result.Value as List<CompanyMaster>;
            int resultCount = resultList.Count;
            //Assert
            Assert.AreEqual(expectTotalCompanyCount, resultCount);
        }
        [TestMethod()]
        public void GetAllCompany_NoContent_Test()
        {
            //Arrange
            var noContentResult = typeof(NoContentResult);
            //Action
            var objEmployeeMasterController = new EmployeeMastersController(_context);
            var apiResult = objEmployeeMasterController.GetAllCompany();
            var resultList = apiResult.Result.Result;
            //Assert
            Assert.AreEqual(noContentResult, resultList);

        }
        #endregion

        #region Unit Test Methode Of GetAllDesignation API
        [TestMethod()]
        public void GetAllDesignation_TotalCount_Test()
        {
            //Arrange
            int expectTotalDesignationCount = 5;
            //Action
            var objEmployeeMasterController = new EmployeeMastersController(_context);
            var apiResult = objEmployeeMasterController.GetAllDesignation();
            var resultList = apiResult.Result.Value as List<DesignatioMaster>;
            int resultCount = resultList.Count;
            //Assert
            Assert.AreEqual(expectTotalDesignationCount, resultCount);
        }

        [TestMethod()]
        public void GetAllDesignation_NoContentReult_Test()
        {
            //Arrange
            var noContentResult = typeof(NoContentResult);
            //Action
            var objEmployeeMasterController = new EmployeeMastersController(_context);
            var apiResult = objEmployeeMasterController.GetAllDesignation();
            var resultList = apiResult.Result.Result;
            //Assert
            Assert.AreEqual(noContentResult, resultList);
        }
        #endregion

        #region Unit Test Methode Of PostEmployeeMaster API

        [TestMethod()]
        public void PostEmployeeMaster_Save_With_ValidData_Test()
        {
            //Arrange
            var objEmployee = new EmployeeMaster();
            objEmployee.EmployeeId = "45678ui2";
            objEmployee.EmpCompanyId = 7;
            objEmployee.EmpFirstName = "Rajat";
            objEmployee.EmpLastName = "Padhy";
            objEmployee.EmpGender = "Male";
            objEmployee.EmpJoingDate = DateTime.Now;
            objEmployee.EmpDesignationId = 10;
            objEmployee.EmpHourlySalaryRate = 50;
           // objEmployee.EmpResinationDate = DateTime.Parse("2020-01-09");

            //Action
            var objEmployeeMasterController = new EmployeeMastersController(_context);
            var Result = objEmployeeMasterController.PostEmployeeMaster(objEmployee);

            //Assert
            Assert.IsInstanceOfType(Result.Result.Result, typeof(CreatedAtActionResult));
        }

        [TestMethod()]
        public void PostEmployeeMaster_Check_With_InValid_ID_Length_Test()
        {
            //Arrange
            var objEmployee = new EmployeeMaster();
            objEmployee.EmployeeId = "ATIL12323234567234567890234567asdfghjk8";
            objEmployee.EmpCompanyId = 7;
            objEmployee.EmpFirstName = "ChitoRanjan";
            objEmployee.EmpLastName = "Pradhanasdfghjkl";
            objEmployee.EmpGender = "Male";
            objEmployee.EmpJoingDate = DateTime.Now;
            objEmployee.EmpDesignationId = 10;
            objEmployee.EmpHourlySalaryRate = 50;

            //Action
            var objEmployeeMasterController = new EmployeeMastersController(_context);
            var apiResult = objEmployeeMasterController.PostEmployeeMaster(objEmployee);

            //Assert
            Assert.IsInstanceOfType(apiResult.Result.Result, typeof(BadRequestResult));
        }

        [TestMethod()]
        public void PostEmployeeMaster_Check_With_Missing_DataForSome_Column_Test()
        {
            //Arrange
            var objEmployee = new EmployeeMaster();
            objEmployee.EmployeeId = "ATIL450";
            objEmployee.EmpCompanyId = 7;
            objEmployee.EmpFirstName = "";
            objEmployee.EmpLastName = "";
            objEmployee.EmpGender = "Male";
            objEmployee.EmpJoingDate = DateTime.Now;
            objEmployee.EmpDesignationId = 10;
            objEmployee.EmpHourlySalaryRate = 50;

            //Action
            var objEmployeeMasterController = new EmployeeMastersController(_context);
            var apiResult = objEmployeeMasterController.PostEmployeeMaster(objEmployee);

            //Assert
            Assert.IsInstanceOfType(apiResult.Result.Result, typeof(BadRequestResult));
        }


        [TestMethod()]  
        public void PostEmployeeMaster_Check_With_Duplicate_EmpID_ReturnConflict_Test()
        {
            //Arrange
            var objEmployee = new EmployeeMaster();
            objEmployee.EmployeeId = "ATIL18712";
            objEmployee.EmpCompanyId = 7;
            objEmployee.EmpFirstName = "ChitoRanjanP";
            objEmployee.EmpLastName = "PradhanaP";
            objEmployee.EmpGender = "Male";
            objEmployee.EmpJoingDate = DateTime.Now;
            objEmployee.EmpDesignationId = 10;
            objEmployee.EmpHourlySalaryRate = 50;

            //Action
            var objEmployeeMasterController = new EmployeeMastersController(_context);
            var apiPostEmp = objEmployeeMasterController.PostEmployeeMaster(objEmployee);
           
            //Assert
            Assert.IsInstanceOfType(apiPostEmp.Result.Result, typeof(StatusCodeResult));
        }

        [TestMethod()]
        public void PostEmployeeMaster_Save_With_InValid_ResignedDate_Test()
        {
            //Arrange
            var objEmployee = new EmployeeMaster();
            objEmployee.EmployeeId = "Adewd56cw";
            objEmployee.EmpCompanyId = 7;
            objEmployee.EmpFirstName = "Chito";
            objEmployee.EmpLastName = "Pradhan";
            objEmployee.EmpGender = "Male";
            objEmployee.EmpJoingDate = DateTime.Now;
            objEmployee.EmpResinationDate = DateTime.Parse("2020-01-09");
            objEmployee.EmpDesignationId = 7;
            objEmployee.EmpHourlySalaryRate = 50;

            //Action
            var objEmployeeMasterController = new EmployeeMastersController(_context);
            var apiResult = objEmployeeMasterController.PostEmployeeMaster(objEmployee);

            //Assert
            Assert.IsInstanceOfType(apiResult.Result.Result, typeof(BadRequestResult));
        }
        [TestMethod()]
        public void PostEmployeeMaster_Save_With_Valid_ResignedDate_Test()
        {
            //Arrange
            var objEmployee = new EmployeeMaster();
            objEmployee.EmployeeId = "ATIL1533c";
            objEmployee.EmpCompanyId = 7;
            objEmployee.EmpFirstName = "Chito";
            objEmployee.EmpLastName = "Pradhan";
            objEmployee.EmpGender = "Male";
            objEmployee.EmpJoingDate = DateTime.Now;
            objEmployee.EmpResinationDate = DateTime.Parse("2024-01-09");
            objEmployee.EmpDesignationId = 7;
            objEmployee.EmpHourlySalaryRate = 50;

            //Action
            var objEmployeeMasterController = new EmployeeMastersController(_context);
            var apiResult = objEmployeeMasterController.PostEmployeeMaster(objEmployee);

            //Assert
            Assert.IsInstanceOfType(apiResult.Result.Result, typeof(CreatedAtActionResult));
        }
        #endregion

        #region Unit Test Methode Of PutEmployeeMaster API
        [TestMethod()]
        public void PutEmployeeMaster_Update_With_ValidData_Test()
        {
            //Arrange
            var objEmployee = new EmployeeMaster();
            objEmployee.EmployeeId = "45678ui";
            objEmployee.EmpCompanyId = 7;
            objEmployee.EmployeeKey = 10106;
            objEmployee.EmpFirstName = "ChitaR";
            objEmployee.EmpLastName = "Pradhan";
            objEmployee.EmpGender = "Male";
            objEmployee.EmpJoingDate = DateTime.Now;
            objEmployee.CreationDate = new DateTime(2022, 11, 10, 12,52, 25, 470);
            objEmployee.EmpDesignationId = 10;
            objEmployee.EmpHourlySalaryRate = 50;

            //Action
            var objEmployeeMasterController = new EmployeeMastersController(_context);
            var apiResult = objEmployeeMasterController.PutEmployeeMaster(objEmployee.EmployeeKey, objEmployee);

            //Assert
            Assert.IsInstanceOfType(apiResult.Result.Result, typeof(OkObjectResult));
        }

        [TestMethod()]
        public void PutEmployeeMaster_Check_With_Duplicate_EmpID_Test()
        {
            //Arrange
            var objEmployee = new EmployeeMaster();
            objEmployee.EmployeeId = "ATIL189";
            objEmployee.EmpCompanyId = 7;
            objEmployee.EmployeeKey = 29;
            objEmployee.EmpFirstName = "Chitaranjan";
            objEmployee.EmpLastName = "Pradhan";
            objEmployee.EmpGender = "Male";
            objEmployee.EmpJoingDate = DateTime.Now;
            objEmployee.CreationDate = new DateTime(2022, 11, 10, 12, 52, 25, 470);
            objEmployee.EmpDesignationId = 10;
            objEmployee.EmpHourlySalaryRate = 50;

            //Action
            var objEmployeeMasterController = new EmployeeMastersController(_context);
            var apiPostEmp = objEmployeeMasterController.PutEmployeeMaster(objEmployee.EmployeeKey, objEmployee);

            //Assert
            Assert.IsInstanceOfType(apiPostEmp.Result.Result, typeof(StatusCodeResult));
        }

        [TestMethod()]
        public void PutEmployeeMaster_Check_With_Missing_DataForSome_Column_Test()
        {
            //Arrange
            var objEmployee = new EmployeeMaster();
            objEmployee.EmployeeId = "4567895678ui";
            objEmployee.EmpCompanyId = 7;
            objEmployee.EmployeeKey = 10095;
            objEmployee.EmpFirstName = "";
            objEmployee.EmpLastName = "";
            objEmployee.EmpGender = "Male";
            objEmployee.EmpJoingDate = DateTime.Now;
            objEmployee.CreationDate = new DateTime(2022, 11, 10, 12, 52, 25, 470);
            objEmployee.EmpDesignationId = 10;
            objEmployee.EmpHourlySalaryRate = 50;

            //Action
            var objEmployeeMasterController = new EmployeeMastersController(_context);
            var apiResult = objEmployeeMasterController.PutEmployeeMaster(objEmployee.EmployeeKey,objEmployee);

            //Assert
            Assert.IsInstanceOfType(apiResult.Result.Result, typeof(BadRequestResult));
        }

        [TestMethod()]
        public void PutEmployeeMaster_Check_With_InValid_ID_Length_Test()
        {
            //Arrange
            var objEmployee = new EmployeeMaster();
            objEmployee.EmployeeId = "ATIL12323234567234567890234567asdfghjk8";
            objEmployee.EmpCompanyId = 7;
            objEmployee.EmployeeKey = 10095;
            objEmployee.EmpFirstName = "ChitoRanjan";
            objEmployee.EmpLastName = "Pradhanasdfghjkl";
            objEmployee.EmpGender = "Male";
            objEmployee.EmpJoingDate = DateTime.Now;
            objEmployee.CreationDate = new DateTime(2022, 11, 10, 12, 52, 25, 470);
            objEmployee.EmpDesignationId = 10;
            objEmployee.EmpHourlySalaryRate = 50;

            //Action
            var objEmployeeMasterController = new EmployeeMastersController(_context);
            var Result = objEmployeeMasterController.PutEmployeeMaster(objEmployee.EmployeeKey, objEmployee);

            //Assert
            Assert.IsInstanceOfType(Result.Result.Result, typeof(BadRequestResult));
        }

        [TestMethod()]
        public void PutEmployeeMaster_Save_With_Valid_ResignedDate_Test()
        {
            //Arrange
            var objEmployee = new EmployeeMaster();
            objEmployee.EmployeeId = "ATIL2001";
            objEmployee.EmpCompanyId = 1014;
            objEmployee.EmpFirstName = "Demo";
            objEmployee.EmpLastName = "Demo";
            objEmployee.EmpGender = "Male";
            objEmployee.EmpJoingDate =new DateTime(2022,10,04 ,00,00,00,000);
            objEmployee.EmpResinationDate = new DateTime(2023,12,02,10,0,0);
            objEmployee.EmpDesignationId = 7;
            objEmployee.EmpHourlySalaryRate = 50;
            objEmployee.EmployeeKey = 10106;
            objEmployee.CreationDate = new DateTime(2022, 10, 04, 12, 16, 08, 367);

            //Action
            var objEmployeeMasterController = new EmployeeMastersController(_context);
            var Result = objEmployeeMasterController.PutEmployeeMaster(objEmployee.EmployeeKey,objEmployee);

            //Assert
            Assert.IsInstanceOfType(Result.Result.Result,typeof(OkObjectResult));
        }
        [TestMethod()]
        public void PutEmployeeMaster_Save_With_InValid_ResignedDate_Test()
        {
            //Arrange
            var objEmployee = new EmployeeMaster();
            objEmployee.EmployeeId = "atil777";
            objEmployee.EmpCompanyId = 1014;
            objEmployee.EmpFirstName = "Chito";
            objEmployee.EmpLastName = "Pradhan";
            objEmployee.EmpGender = "Male";
            objEmployee.EmpJoingDate = new DateTime(2022, 10, 04, 00, 00, 00, 000);
            objEmployee.EmpResinationDate = new DateTime(2020, 12, 02, 10, 0, 0);
            objEmployee.EmpDesignationId = 7;
            objEmployee.EmpHourlySalaryRate = 50;
            objEmployee.EmployeeKey = 103;
            objEmployee.CreationDate = new DateTime(2022, 10, 04, 12, 16, 08, 367);

            //Action
            var objEmployeeMasterController = new EmployeeMastersController(_context);
            var Result = objEmployeeMasterController.PutEmployeeMaster(objEmployee.EmployeeKey, objEmployee);

            //Assert
            Assert.IsInstanceOfType(Result.Result.Result, typeof(BadRequestResult));
        }

        [TestMethod()]
        public void PutEmployeeMaster_Check_With_Invalid_EmployeeKey_Test()
        {
            //Arrange
            var objEmployee = new EmployeeMaster();
            objEmployee.EmployeeId = "atil777";
            objEmployee.EmpCompanyId = 1014;
            objEmployee.EmpFirstName = "Chito";
            objEmployee.EmpLastName = "Pradhan";
            objEmployee.EmpGender = "Male";
            objEmployee.EmpJoingDate = new DateTime(2022, 10, 04, 00, 00, 00, 000);
            objEmployee.EmpResinationDate = new DateTime(2023, 12, 02, 10, 0, 0);
            objEmployee.EmpDesignationId = 7;
            objEmployee.EmpHourlySalaryRate = 50;
            objEmployee.EmployeeKey = 6789;
            objEmployee.CreationDate = new DateTime(2022, 10, 04, 12, 16, 08, 367);

            //Action
            var objEmployeeMasterController = new EmployeeMastersController(_context);
            var Result = objEmployeeMasterController.PutEmployeeMaster(objEmployee.EmployeeKey, objEmployee);

            //Assert
            Assert.IsInstanceOfType(Result.Result.Result, typeof(BadRequestResult));
        }
        #endregion

        #region Unit Test Methode Of DeleteEmployeeMaster API
        [TestMethod()]
        public void DeleteEmployeeMaster_invalid_employee_key_Test()
        {
            //Arrange
            long empKeyInput = 1;

            //Action
            var objEmployeeMasterController = new EmployeeMastersController(_context);
            var result = objEmployeeMasterController.DeleteEmployeeMaster(empKeyInput);

            //Assert
            Assert.IsInstanceOfType(result.Result.Result, typeof(NotFoundResult));
        }

        [TestMethod()]
        public void DeleteEmployeeMaster_Valid_employee_key_Test()
        {
            //Arrange
            long empKeyInput = 10111;

            //Action
            var objEmployeeMasterController = new EmployeeMastersController(_context);
            var result = objEmployeeMasterController.DeleteEmployeeMaster(empKeyInput);

            //Assert
            Assert.IsInstanceOfType(result.Result.Result ,typeof(ObjectResult));
        }

        [TestMethod()]
        public void GetCheck_Used_Employeekey_Test()
        {
            //Arrange
            long empKeyInput = 19;

            //Action
            var objEmployeeMasterController = new EmployeeMastersController(_context);
            var result = objEmployeeMasterController.GetEmployeeKeyInUse(empKeyInput);
            var empResult = result.Result.Value;

            //Assert
            Assert.IsTrue(empResult);
        }

        [TestMethod()]
        public void GetCheck_UnUsed_Employeekey_Test()
        {
            //Arrange
            long empKeyInput = 103;

            //Action
            var objEmployeeMasterController = new EmployeeMastersController(_context);
            var result = objEmployeeMasterController.GetEmployeeKeyInUse(empKeyInput);
            var empresult = result.Result.Value;

            //Assert
            Assert.IsFalse(empresult);
        }


        #endregion
    }
}