using System.Collections.Generic;
using NUnit.Framework;
using Wbcl.Clients.TgClient.MarkupUtils;
using Wbcl.Core.Models.Database;

namespace Wbcl.Tests.TgClient
{
    [TestFixture]
    class AddNewalarmsTests
    {

        [Test]
        public void GetReplyKeyboardForGroups_Success_NotEmpty()
        {
            //Arrange
            var preferences = new List<UserPreference>()
            {
                new() {TargetId = 111, TargetName = "First group"},
                new() {TargetId = 222, TargetName = "Вторая группа"},
            };

            //Act
            var resultKeyboard = MessageMarkupUtilities.GetReplyKeyboardForGroups(preferences);

            //Assert
            Assert.That(resultKeyboard, Is.Not.Empty);
            Assert.That(resultKeyboard.Count, Is.EqualTo(2));
            Assert.That(resultKeyboard[0][0].Text, Is.EqualTo("First group (id: 111)"));
            Assert.That(resultKeyboard[1][0].Text, Is.EqualTo("Вторая группа (id: 222)"));
        }

        [Test]
        public void GetReplyKeyboardForGroups_Success_Empty()
        {
            //Arrange
            var preferences = new List<UserPreference>();

            //Act
            var resultKeyboard = MessageMarkupUtilities.GetReplyKeyboardForGroups(preferences);

            //Assert
            Assert.That(resultKeyboard, Is.Empty);
        }
    }
}
