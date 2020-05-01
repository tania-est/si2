using AutoMapper;
using Microsoft.Extensions.Logging;
using Moq;
using NUnit.Framework;
using si2.bll.Dtos.Results.Simplecohort;
using si2.bll.Helpers;
using si2.bll.Services;
using si2.dal.Entities;
using si2.dal.UnitOfWork;
using System;
using System.Threading;
using System.Threading.Tasks;
using static si2.common.Enums;

namespace si2.tests.Services
{
    [TestFixture]
    public class SimpleCohortTest
    {
        private readonly Mock<IUnitOfWork> _mockUnitOfWork;
        private readonly Mock<ILogger<SimplecohortService>> _mockLogger;
        private readonly IMapper _mapper;

        private SimplecohortDto mockSimpleCohortDto = new SimplecohortDto()
        {
            Id = new Guid("31B389D0-4A20-4978-DDCB-08D7C432FC14"),
            registrationRequirements = "timberland",
            Status = SimplecohortStatus.Started.ToString(),
            graduationRequirements = "I like tags",
            transferRequirements = "I like tags",
            RowVersion = Convert.FromBase64String("AAAAAAAAB94=")
        };

        private Simplecohort mockSimpleCohort = new Simplecohort()
        {
            Id = new Guid("31B389D0-4A20-4978-DDCB-08D7C432FC14"),
            registrationRequirements = "timberland",
            Status = SimplecohortStatus.Started,
            graduationRequirements = "I like tags",
            transferRequirements = "I like tags",
            RowVersion = Convert.FromBase64String("AAAAAAAAB94=")
        };

        private si2.bll.Dtos.Requests.Simplecohort.UpdateSimplecohortDto mockUpdatedSimpleCohortDto = new si2.bll.Dtos.Requests.Simplecohort.UpdateSimplecohortDto()
        {
            registrationRequirements = "timberland_new",
            graduationRequirements = "I like new tags",
            transferRequirements = "I like new tags",
            RowVersion = Convert.FromBase64String("AAAAAAAAB94=")
        };

        private Simplecohort mockUpdatedSimpleCohort = new Simplecohort()
        {
            Id = new Guid("31B389D0-4A20-4978-DDCB-08D7C432FC14"),
            registrationRequirements = "timberland_new",
            Status = SimplecohortStatus.Started,
            graduationRequirements = "I like new tags",
            transferRequirements = "I like new tags",
            RowVersion = Convert.FromBase64String("AAAAAAAAB94=")
        };

        public ISimplecohortService _simpleCohortService;


        public SimpleCohortTest()
        {
            _mockUnitOfWork = new Mock<IUnitOfWork>();
            _mockLogger = new Mock<ILogger<SimplecohortService>>();
            _mapper = new MapperConfiguration(mc => { mc.AddProfile(new MappingProfile()); }).CreateMapper();

            _simpleCohortService = new SimplecohortService(_mockUnitOfWork.Object, _mapper, _mockLogger.Object);
        }

        [Test]
        public void GetSimplecohortByIdAsync_WhenMatching()
        {
            // Arrange
            _mockUnitOfWork.Setup(_mockUnitOfWork => _mockUnitOfWork.Simplecohorts.GetAsync(mockSimpleCohortDto.Id, It.IsAny<CancellationToken>()))
                .Returns(Task.FromResult(mockSimpleCohort));

            // Act
            var expected = _simpleCohortService.GetSimplecohortByIdAsync(mockSimpleCohortDto.Id, It.IsAny<CancellationToken>()).Result;

            // Assert

            Assert.AreEqual(expected, mockSimpleCohortDto);
        }

        [Test]
        public void UpdateSimplecohortAsync()
        {
            // Arrange
            var updatedEntity = _mapper.Map<Simplecohort>(mockUpdatedSimpleCohortDto);
            updatedEntity.Id = mockSimpleCohortDto.Id;

            _mockUnitOfWork.Setup(_mockUnitOfWork => _mockUnitOfWork.Simplecohorts.UpdateAsync(updatedEntity, mockSimpleCohortDto.Id, It.IsAny<CancellationToken>(),
                                                             updatedEntity.RowVersion))
                            .Returns(Task.FromResult(mockSimpleCohort));

            _mockUnitOfWork.Setup(_mockUnitOfWork => _mockUnitOfWork.Simplecohorts.GetAsync(mockSimpleCohort.Id, It.IsAny<CancellationToken>()))
                .Returns(Task.FromResult(mockUpdatedSimpleCohort));

            var simplecohortDto = _mapper.Map<SimplecohortDto>(mockUpdatedSimpleCohort);
            Task<SimplecohortDto> dt = Task.FromResult<SimplecohortDto>(simplecohortDto);
            // Act
            var expected = _simpleCohortService.UpdateSimplecohortAsync(mockSimpleCohortDto.Id, mockUpdatedSimpleCohortDto, It.IsAny<CancellationToken>()).Result;

            // Assert
            Assert.AreEqual(expected.registrationRequirements, dt.Result.registrationRequirements);
        }

        [Test]
        public void DeleteSimplecohortAsync()
        {
            // Arrange
            _mockUnitOfWork.Setup(_mockUnitOfWork => _mockUnitOfWork.Simplecohorts.FirstAsync(c => c.Id == mockSimpleCohortDto.Id, It.IsAny<CancellationToken>()))
                    .Returns(Task.FromResult(mockSimpleCohort));

            var entitySimplecohort = _mapper.Map<Simplecohort>(mockSimpleCohort);

            _mockUnitOfWork.Setup(_mockUnitOfWork => _mockUnitOfWork.Simplecohorts.DeleteAsync(entitySimplecohort, It.IsAny<CancellationToken>()))
                .Returns(Task.FromResult(mockSimpleCohort));

            var simplecohortDto = _mapper.Map<SimplecohortDto>(mockSimpleCohort);
            Task<SimplecohortDto> dt = Task.FromResult<SimplecohortDto>(simplecohortDto);

            // Act
            var expected = _simpleCohortService.DeleteSimplecohortByIdAsync(dt.Result.Id, It.IsAny<CancellationToken>());

            var res = _mockUnitOfWork.Object.Simplecohorts.GetAsync(mockSimpleCohort.Id, It.IsAny<CancellationToken>());

            // Assert0
           Assert.IsNull(res.Result);
        }

        [Test]
        public void DeleteSimplecohortAsync_ServiceCall()
        {
            // Set up the uow’s Delete call
            _mockUnitOfWork.Setup(_mockUnitOfWork => _mockUnitOfWork.Simplecohorts.FirstAsync(c => c.Id == mockSimpleCohortDto.Id, It.IsAny<CancellationToken>()))
                 .Returns(Task.FromResult(mockSimpleCohort));

            // Act
           /*var value =  _mockUnitOfWork.Object.Simplecohorts.DeleteAsync(mockSimpleCohort, It.IsAny<CancellationToken>());

            _mockUnitOfWork.Object.SaveChangesAsync(It.IsAny<CancellationToken>());
            */

            var expected = _simpleCohortService.DeleteSimplecohortByIdAsync(mockSimpleCohortDto.Id, It.IsAny<CancellationToken>());

            // Assert
            // verify that the Delete method we set up above was called
            // with the mockSimpleCohort as the first argument
            //_mockUnitOfWork.Verify(_mockUnitOfWork => _mockUnitOfWork.Simplecohorts.DeleteAsync(mockSimpleCohort, It.IsAny<CancellationToken>()));
            
            //_mockUnitOfWork.Verify(_mockUnitOfWork => _mockUnitOfWork.Simplecohorts.DeleteAsync(mockSimpleCohort, It.IsAny<CancellationToken>()));
            //Assert.AreEqual(expected, mockSimpleCohortDto.Id);

            Assert.IsNotNull(expected);
        }

        [Test]
        public void CreateSimplecohortAsync()
        {
            _mockUnitOfWork.Setup(_mockUnitOfWork => _mockUnitOfWork.Simplecohorts.AddAsync(mockSimpleCohort, It.IsAny<CancellationToken>()))
                 .Returns(Task.FromResult(mockSimpleCohortDto));

            Assert.IsNotNull(mockSimpleCohortDto);
        }

        [Test]
        public void RetrieveSimplecohortAsync()
        {
            Simplecohort simpleCohort = new Simplecohort()
            {
                Id = new Guid("31B389D0-4A20-4978-DDCB-08D7C432FC14"),
                registrationRequirements = "timberland",
                Status = SimplecohortStatus.Started,
                graduationRequirements = "I like tags",
                transferRequirements = "I like tags",
                RowVersion = Convert.FromBase64String("AAAAAAAAB94=")
            };

            _mockUnitOfWork.Setup(_mockUnitOfWork => _mockUnitOfWork.Simplecohorts.GetAsync(mockSimpleCohort.Id, It.IsAny<CancellationToken>()))
                .Returns(Task.FromResult(simpleCohort));


            // Assert
            Assert.IsNotNull(simpleCohort);
            Assert.That(simpleCohort.Id, Is.EqualTo(mockSimpleCohort.Id));
        }

    }
}