using NUnit.Framework;
using si2.bll.Helpers.Credits;
using si2.dal.Entities;
using si2.common;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Logging;
using si2.api.Controllers;
using si2.bll.Services;
using System.Threading.Tasks;
using Moq;
using si2.dal.Repositories;
using System.Threading;
using si2.bll.Dtos.Requests.Simplecohort;
using si2.bll.Dtos.Results.Simplecohort;
using si2.dal.UnitOfWork;

namespace si2.tests
{
    public class SimplecohortsTest
    {
        [SetUp]
        public void Setup()
        {
            LinkGenerator _linkGenerator;
            ILogger<SimplecohortsController> _logger;
            ISimplecohortService _simplecohortService;
            CreateSimplecohortDto createSimplecohortDto;
            CancellationToken ct;
            SimplecohortsController contr;
        }

        [Test]
        public void Test1()
        {
            Assert.Pass();
        }


       /* [Test]
        public void TestSimplecohortsContructor()
        {
            var controller = new si2.api.Controllers.SimplecohortsController();
            Assert.AreEqual("ABC", u.getCode());
            Assert.AreEqual("B", u.getName());
        }
        */

        // [Fact]
        [Test]
        public async Task ReturnSimplecohortForCreateSimplecohortDto()          //<-- note test is now async as well
        {   
            //Arrange

            /* var _mockAreaRepository = new Mock<ISimplecohortRepository>();
             _mockAreaRepository
                 .Setup(x => x.AddArea(testArea));

             _mockAreaRepository
                 .Setup(x => x.SaveChangesAsync())
                 .ReturnsAsync(true); //<-- returns completed Task<bool> when invoked

             var _mockMapper = new Mock<IMapper>();
             _mockMapper
                 .Setup(_ => _.Map<Area>(It.IsAny<AreaForCreationDto>()))
                 .Returns(testArea);
             _mockMapper
                 .Setup(_ => _.Map<AreaDto>(It.IsAny<Area>()))
                 .Returns(testAreaDto);*/

            var _mockAreaService = new Mock<ISimplecohortService>();
            var _mockLogger = new Mock<ILogger<SimplecohortsController>>();

           /* var _contr = new SimplecohortsController(_linkGenerator.Object, _mockLogger.Object, _mockAreaService.Object);

            // Act
            var result = await _contr.CreateSimplecohort(createSimplecohortDto);    //<-- await 

            // Assert
            Assert.NotNull(result);
            var objectResult = Assert.IsType<CreatedAtRouteResult>(result);
            var model = Assert.IsAssignableFrom<SimplecohortDto>(objectResult.Value);
            var SimplecohortStatus = model.Status;
            Assert.Equals("Test Simplecohort For Creation", SimplecohortStatus);*/
        }


        [Test]
        public void TestGetSimplecohort()
        {
            //Arrange
            var _mockUnitOfWork = new Mock<IUnitOfWork>();
            var _mockServiceLayer = new Mock<ISimplecohortService>(_mockUnitOfWork.Object);
            var simpleCohort = new Simplecohort();
            var simpleCohortDto = new SimplecohortDto();
            //_mockServiceLayer.Setup(s => s.GetSimplecohortByIdAsync(System.Guid.NewGuid(), CancellationToken.None));//.Returns(simpleCohortDto);
            _mockServiceLayer.Setup(e => e.GetSimplecohortByIdAsync(It.IsAny<System.Guid>(), CancellationToken.None));
            
            System.Guid id = new System.Guid("35ca81b9 - f1d4 - 41f5 - 89a2 - 08d7da0239d0");

            //Verify that Add was never called with an UserMetaData with FirstName != "FirstName1":
           // _mockServiceLayer.Verify(e => e.GetSimplecohortByIdAsync(It.Is<SimplecohortDto>(d => d.FirstName != "FirstName1")), Times.Never());

            var _mockLogger = new Mock<ILogger<SimplecohortsController>>();
          //  var _controller = new SimplecohortsController(_linkGenerator.Object, _mockLogger.Object, _mockServiceLayer.Object);

            //Act
         //   var viewResult = _controller.GetSimplecohort(id,CancellationToken.None) as ViewResult;

            //Assert
           // Assert.AreEqual(simpleCohort, viewResult.Model);
        }

    }
}
 