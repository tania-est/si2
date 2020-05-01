using AutoMapper;
using Microsoft.Extensions.Logging;
using Moq;
using NUnit.Framework;
using si2.bll.Dtos.Results.Dataflow;
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
    public class DataFlowTest
    {
        private readonly Mock<IUnitOfWork> _mockUnitOfWork;
        private readonly Mock<ILogger<DataflowService>> _mockLogger;
        private readonly IMapper _mapper;

        private DataflowDto mockDataFlowDto = new DataflowDto()
        {
            Id = new Guid("31B389D0-4A20-4978-DDCB-08D7C432FC14"),
            Name = "timberland",
            Status = DataflowStatus.Started.ToString(),
            Tag = "I like tags",
            Title = "To be or not to be, that is the answer",
            RowVersion = Convert.FromBase64String("AAAAAAAAB94=")
        };

        private Dataflow mockDataFlow = new Dataflow()
        {
            Id = new Guid("31B389D0-4A20-4978-DDCB-08D7C432FC14"),
            Name = "timberland",
            Status = DataflowStatus.Started,
            Tag = "I like tags",
            Title = "To be or not to be, that is the answer",
            RowVersion = Convert.FromBase64String("AAAAAAAAB94=")
        };

        public IDataflowService _dataFlowService;

        public DataFlowTest()
        {
            _mockUnitOfWork = new Mock<IUnitOfWork>();
            _mockLogger = new Mock<ILogger<DataflowService>>();
            _mapper = new MapperConfiguration(mc => { mc.AddProfile(new MappingProfile()); }).CreateMapper();

            _dataFlowService = new DataflowService(_mockUnitOfWork.Object, _mapper, _mockLogger.Object);
        }

        [Test]
        public void GetDataflowByIdAsync_WhenMatching()
        {
            // Arrange
            _mockUnitOfWork.Setup(_mockUnitOfWork => _mockUnitOfWork.Dataflows.GetAsync(mockDataFlowDto.Id, It.IsAny<CancellationToken>()))
                .Returns(Task.FromResult(mockDataFlow));

            // Act
            var expected = _dataFlowService.GetDataflowByIdAsync(mockDataFlowDto.Id, It.IsAny<CancellationToken>()).Result;

            // Assert
            Assert.AreEqual(expected, mockDataFlowDto);
        }
    }
}