using Moq;
using NUnit.Framework;
using Tour_Planner.Extensions;
using Tour_Planner.Models;
using Tour_Planner.Services.Interfaces;
using Tour_Planner.ViewModels.TourLogs;
using Tour_Planner.ViewModels.Tours;

namespace Test.Tour_Planner.Services.DialogTests
{
    public class DialogTestsInTourLogsViewModel
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
            _viewModel = new TourLogsViewModel(_dialogMock.Object, _restMock.Object, _mediatorMock.Object)
                {
                    SelectedTourLog = new TourLog(),
                    Tour = new Tour()
                };
        }

        [Test]
        public void DisplayAddTourCommand_ShowsMessageDialog()
        {
            _dialogMock.Setup(service => service.ShowDialog(It.IsAny<AddTourLogViewModel>())).Verifiable();
            _viewModel.DisplayAddTourLogCommand.Execute(null);
            _dialogMock.Verify();
        }
        [Test]
        public void DisplayEditTourCommand_ShowsMessageDialog()
        {
            _dialogMock.Setup(service => service.ShowDialog(It.IsAny<EditTourLogViewModel>())).Verifiable();
            _viewModel.DisplayEditTourLogCommand.Execute(null);
            _dialogMock.Verify();
        }
    }
}