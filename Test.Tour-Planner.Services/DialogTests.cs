using Moq;
using NUnit.Framework;
using Tour_Planner.Services.Interfaces;
using Tour_Planner.ViewModels;

namespace Test.Tour_Planner.Services
{
    public class DialogTests
    {
        private Mock<IDialogService> _mock;
        private HomeViewModel _viewModel;
        [SetUp]
        public void Setup()
        {
            _mock = new Mock<IDialogService>();
            _viewModel = new HomeViewModel(_mock.Object);
        }

        [Test]
        public void DisplayMessageCommand_ShowsMessageDialog()
        {
            _mock.Setup(service => service.ShowDialog(It.IsAny<AddTourViewModel>())).Verifiable();
            _viewModel.DisplayMessageCommand.Execute(null);
            _mock.Verify();
        }
    }
}