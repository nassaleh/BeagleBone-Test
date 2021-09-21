using System.Device.Gpio;
using BeagleBone;
using Moq;
using NUnit.Framework;
using Sqlite;

namespace UnitTests
{
    public class Tests
    {
        [SetUp]
        public void Setup()
        {
        }

        [Test]
        public void PinManager_CallInitialize_RegisersCallbacks()
        {
            //Arrange
            var dbContext = new Mock<IDBHelper>();
            var pinReader = new Mock<IPinReader>();
            int[] pins = { 1, 2, 3, 4 };


            PinManager pm = new PinManager(pins, pinReader.Object, dbContext.Object);

            //Act
            pm.Initialize();

            //Assert
            pinReader.Verify(x => x.RegisterPinsForCallback(pins, It.IsAny<PinChangeEventHandler>()),
                Times.Once(),
                "Method called the wrong number of times"
            );

        }
    }
}