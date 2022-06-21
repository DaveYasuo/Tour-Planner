using Moq;
using NUnit.Framework;
using Tour_Planner.Extensions;
using Tour_Planner.Models;
using Tour_Planner.Services.Interfaces;
using Tour_Planner.ViewModels.Tours;

namespace Test.Tour_Planner.Services.DialogTests
{
    public class DialogTestsInListToursViewModel
    {
        private Mock<IDialogService> _dialogMock;
        private Mock<IRestService> _restMock;
        private Mock<IMediator> _mediatorMock;
        private ListToursViewModel _viewModel;
        [SetUp]
        public void Setup()
        {
            _dialogMock = new Mock<IDialogService>();
            _restMock = new Mock<IRestService>();
            _mediatorMock = new Mock<IMediator>();
            _viewModel = new ListToursViewModel(_dialogMock.Object, _restMock.Object, _mediatorMock.Object, ".\\..\\..\\..\\Images\\");
        }

        [Test]
        public void DisplayAddTourCommand_ShowsMessageDialog()
        {
            _dialogMock.Setup(service => service.ShowDialog(It.IsAny<AddTourViewModel>())).Verifiable();
            _viewModel.DisplayAddTourCommand.Execute(null);
            _dialogMock.Verify();
        }
        [Test]
        public void DisplayEditTourCommand_ShowsMessageDialog()
        {
            _viewModel.SelectedTour = new Tour();

            _dialogMock.Setup(service => service.ShowDialog(It.IsAny<EditTourViewModel>())).Verifiable();
            _viewModel.DisplayEditTourCommand.Execute(null);
            _dialogMock.Verify();
        }
    }
}