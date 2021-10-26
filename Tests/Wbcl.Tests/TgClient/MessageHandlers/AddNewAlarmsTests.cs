using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Internal;
using Moq;
using NUnit.Framework;
using Telegram.Bot.Types;
using Wbcl.Clients.TgClient.MarkupUtils;
using Wbcl.Clients.TgClient.MessageHandlers.AddNew;
using Wbcl.Core.Models.Database;
using Wbcl.Core.Models.Settings;
using Wbcl.DAL.Context;
using User = Wbcl.Core.Models.Database.User;

namespace Wbcl.Tests.TgClient.MessageHandlers
{
    [TestFixture]
    class AddNewAlarmsTests
    {
        private Settings _settings;
        private Mock<IUsersContext> _mockDb;

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
        }


        [Test]
        public void GetResponseTo_Success_StandardUserEmptyMessage()
        {
            //Arrange
            var prefs = new List<UserPreference>
            {
                new () {User = new User()},
                new () {User = new User()},
                new () {User = new User()},
            }.AsQueryable();

            var mockSet = new Mock<DbSet<UserPreference>>();
            mockSet.As<IQueryable<UserPreference>>().Setup(m => m.Provider).Returns(prefs.Provider);
            mockSet.As<IQueryable<UserPreference>>().Setup(m => m.Expression).Returns(prefs.Expression);
            mockSet.As<IQueryable<UserPreference>>().Setup(m => m.ElementType).Returns(prefs.ElementType);
            mockSet.As<IQueryable<UserPreference>>().Setup(m => m.GetEnumerator()).Returns(prefs.GetEnumerator());
            
            _mockDb
                .Setup(db => db.Preferences)
                .Returns(mockSet.Object);

            var addNewAlarms = new Step1AddNewAlarms(_mockDb.Object, _settings);
            var emptyMessage = new Message {Chat = new Chat {Id = 123}};
            var user = new User
            {
                SubscriptionStatus = UserType.StandardUser,
                State = ChatState.Standrard
            };

            //Act
            var response = addNewAlarms.GetResponseTo(emptyMessage, user);

            //Assert
            Assert.That(response, Is.Not.Null);
            Assert.That(response.ChatId, Is.EqualTo(123));
            Assert.That(response.Text, Is.EqualTo("Введите ссылку на группу, в которой нужно следить за ключевыми словами:"));
            Assert.That(user.State, Is.EqualTo(ChatState.NewGroupToAdd));
        }

        [Test]
        public void GetResponseTo_Fails_StandardUserSubLimitExceeded()
        {
            //Arrange
            var user = new User
            {
                SubscriptionStatus = UserType.StandardUser,
                State = ChatState.Standrard
            };

            var prefs = new List<UserPreference>
            {
                new () {User = user},
                new () {User = user},
                new () {User = user},
            }.AsQueryable();

            _settings.Vkontakte.BaseSubscriptionsLimit = 2;

            var mockSet = new Mock<DbSet<UserPreference>>();
            mockSet.As<IQueryable<UserPreference>>().Setup(m => m.Provider).Returns(prefs.Provider);
            mockSet.As<IQueryable<UserPreference>>().Setup(m => m.Expression).Returns(prefs.Expression);
            mockSet.As<IQueryable<UserPreference>>().Setup(m => m.ElementType).Returns(prefs.ElementType);
            mockSet.As<IQueryable<UserPreference>>().Setup(m => m.GetEnumerator()).Returns(prefs.GetEnumerator());

            _mockDb
                .Setup(db => db.Preferences)
                .Returns(mockSet.Object);

            var addNewAlarms = new Step1AddNewAlarms(_mockDb.Object, _settings);
            var emptyMessage = new Message { Chat = new Chat { Id = 123 } };


            //Act
            var response = addNewAlarms.GetResponseTo(emptyMessage, user);

            //Assert
            Assert.That(response, Is.Not.Null);
            Assert.That(response.ChatId, Is.EqualTo(123));
            Assert.That(response.Text, Is.EqualTo($"На данный момент лимит подписок ограничивается 2" +
                                                  $" группами. Для того, чтобы подписаться на новые уведомления групп, отпишитесь от старых." +
                                                  $"\nВыбери что тебе нужно сделать:"));
            Assert.That(user.State, Is.EqualTo(ChatState.Standrard));
        }
    }
}
