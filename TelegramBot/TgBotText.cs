﻿using System;
using System.Collections.Generic;
using System.Text;

namespace WhisleBotConsole.TelegramBot
{
    static class TgBotText
    {
        public const string SeeNewKeywords = "Видеть посты в вк когда пишут о чём-то мне нужном";
        public const string AddNewSettings = "Добавить новые подписки";
        public const string EditExistingSettings = "Редактировать подписки";
        public const string RemoveSubscriptions = "Удалить подписки";

        public const string ReplyInputIdOrLink = "Введите id или ссылку на группу, в которой нужно следить за ключевыми словами:";
        public const string EditCurrentSubscriptionsLink = "Укажите у каких групп хотите редактировать подписки: ";


        public static string NotifyMeWhenThisWordInGroupAppears(string groupName, string word)
        {
            return $"Уведомлять меня когда в группе {groupName} будет написана фраза \"{word}\"";
        }
    }
}
