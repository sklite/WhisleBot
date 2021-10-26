using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Internal;
using Moq;
using NUnit.Framework;
using Telegram.Bot.Types;
using Telegram.Bot.Types.ReplyMarkups;
using Wbcl.Clients.TgClient.MarkupUtils;
using Wbcl.Clients.TgClient.MessageHandlers.AddNew;
using Wbcl.Core.Models.Database;
using Wbcl.Core.Models.Settings;
using Wbcl.Core.Utils;
using Wbcl.DAL.Context;
using User = Wbcl.Core.Models.Database.User;

namespace Wbcl.Tests.TgClient.MessageHandlers
{
    [TestFixture]
    class InputGroupTests
    {
        private Settings _settings;
        private Mock<IUsersContext> _mockDb;
        private Mock<IVkUtils> _mockVk;

        [SetUp]
        public void Setup()
        {
            _settings = new Settings
            {
                Vkontakte = new VkSettings
                {
                    BaseSubscriptionsLimit = 5,
                    ExtendedSubscriptionsLimit = 120
                }
            };

            _mockDb = new Mock<IUsersContext>();
            _mockDb
                .Setup(db => db.SaveChanges())
                .Returns(10);

            _mockVk = new Mock<IVkUtils>();
        }


        [Test]
        public void GetResponseTo_Success_CorrectUrlNotEmptyMessage()
        {
            //Arrange
            var user = new User
            {
                SubscriptionStatus = UserType.StandardUser,
                State = ChatState.Standrard
            };

            _mockVk
                .Setup(vk => vk.GetObjIdIdByLink(It.IsAny<Uri>()))
                .Returns((true, 122121, "Group name", PreferenceType.VkGroup));

            var addNewAlarms = new Step2InputGroup(_mockDb.Object, _mockVk.Object);
            var userMessage = new Message {Chat = new Chat {Id = 123}, Text = "https://vk.com/21jqofa" };

            //Act
            var response = addNewAlarms.GetResponseTo(userMessage, user);

            //Assert
            Assert.That(response, Is.Not.Null);
            Assert.That(response.ChatId, Is.EqualTo(123));
            Assert.That(response.Text, Is.EqualTo("Введите слова или фразы через запятую, какие следует искать в этой группе. Например _однушка, перекопка, торты, аквариум, аренда_."));
            Assert.That(response.ReplyMarkup.GetType(), Is.EqualTo(typeof(ReplyKeyboardRemove)));
            Assert.That(user.State, Is.EqualTo(ChatState.NewWordToGroupAdd));
        }

        [Test]
        public void GetResponseTo_Fails_CannotGetGroupId()
        {
            //Arrange
            var user = new User
            {
                SubscriptionStatus = UserType.StandardUser,
                State = ChatState.Standrard
            };

            _mockVk
                .Setup(vk => vk.GetObjIdIdByLink(It.IsAny<Uri>()))
                .Returns((false, 0, null, PreferenceType.WrongLink));

            var addNewAlarms = new Step2InputGroup(_mockDb.Object, _mockVk.Object);
            var userMessage = new Message { Chat = new Chat { Id = 123 }, Text = "https://vk.com/21jqofa" };

            //Act
            var response = addNewAlarms.GetResponseTo(userMessage, user);

            //Assert
            Assert.That(response, Is.Not.Null);
            Assert.That(response.ChatId, Is.EqualTo(123));
            Assert.That(response.Text, Is.EqualTo("Не удалось получть id группы\nВыбери что тебе нужно сделать:"));
            Assert.That(response.ReplyMarkup.GetType(), Is.EqualTo(typeof(ReplyKeyboardMarkup)));
            Assert.That(user.State, Is.EqualTo(ChatState.Standrard));
        }
    }
}
