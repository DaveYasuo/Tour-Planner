using System;
using System.Reflection;
using Moq;
using NUnit.Framework;
using Tour_Planner.DataModels.Enums;
using Tour_Planner.Extensions;
using Tour_Planner.Models;
using Tour_Planner.Services.Interfaces;
using Tour_Planner.ViewModels.TourLogs;

namespace Test.Tour_Planner.ViewModels
{
    public class TestTourLogsViewModel
    {
        private Mock<IDialogService> _dialogMock;
        private Mock<IRestService> _restMock;
        private Mock<IMediator> _mediatorMock;
        private TourLogsViewModel _viewModel;

        [SetUp]
        public void Setup()
        {
            _dialogMock = new Mock<IDialogService>();
            _restMock = new Mock<IRestService>();
            _mediatorMock = new Mock<IMediator>();
            _viewModel = new TourLogsViewModel(_dialogMock.Object, _restMock.Object, _mediatorMock.Object);
        }
        [Test]
        public void VmShouldInitializeCommands()
        {
            Assert.IsNotNull(_viewModel.DisplayAddTourLogCommand, "Add Tour Log Command is null");
            Assert.IsNotNull(_viewModel.DeleteTourLogCommand, "Delete Tour Log Command is null");
            Assert.IsNotNull(_viewModel.DisplayEditTourLogCommand, "Display edit Tour Log Command is null");
        }
        [Test]
        public void DeleteTourLogCommandCanBeExecuted()
        {
            Assert.IsTrue(_viewModel.DeleteTourLogCommand.CanExecute(null));
            var tourLog = new TourLog()
            {
                Comment = "",
                DateTime = new DateTime(12, 12, 12, 12, 12, 12),
                Difficulty = Difficulty.easy,
                Distance = 12.12,
                Id = 1,
                Rating = Rating.good,
                TotalTime = new TimeSpan(12, 32, 43),
                TourId = 21
            };
            _viewModel.SelectedTourLog = tourLog;
            Assert.IsTrue(_viewModel.DeleteTourLogCommand.CanExecute(null));
        }
        [Test]
        public void DeleteCmdCannotBeExecutedSelectedItemNull()
        {

            Assert.IsNull(_viewModel.SelectedTourLog);

            MethodInfo methodInfo = typeof(TourLogsViewModel).GetMethod("DeleteTourLog", BindingFlags.NonPublic | BindingFlags.Instance);
            methodInfo!.Invoke(_viewModel, null);

        }
    }
}