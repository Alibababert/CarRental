using System;
using System.Collections.Generic;
using System.Text;
using NUnit.Framework;
using CarRental.ViewModels;

namespace CarRentalUnitTests
{
    class ReturnCarViewModelTests
    {
        private ReturnCarViewModel _viewModel;

        [SetUp]
        public void Setup()
        {
            _viewModel = new ReturnCarViewModel();
        }

        [Test]
        public void OnStopRentingClicked_NonNumericInput_ErrorMessage()
        {
            // Arrange
            _viewModel.Matarställning = "abc";
            _viewModel.BokNr = "1234";

            // Act
            _viewModel.OnStopRentingClicked(null);

            // Assert
            Assert.AreEqual("Båda fälten behöver vara siffror, utan kommatecken", _viewModel.ResultText);
        }

        [Test]
        public void OnStopRentingClicked_EmptyFields_ErrorMessage()
        {
            // Arrange
            _viewModel.Matarställning = "";
            _viewModel.BokNr = "";

            // Act
            _viewModel.OnStopRentingClicked(null);

            // Assert
            Assert.AreEqual("Fyll i värden i båda fälten", _viewModel.ResultText);
        }

    }
}
