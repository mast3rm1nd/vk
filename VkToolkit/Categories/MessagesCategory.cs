﻿#if WINDOWS_PHONE
using System.Net;
#else
using System.Web;

#endif

namespace VkToolkit.Categories
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.Linq;

    using VkToolkit.Enums;
    using VkToolkit.Model;
    using VkToolkit.Utils;

    /// <summary>
    /// Методы для работы с сообщениями.
    /// </summary>
    public class MessagesCategory
    {
        private readonly VkApi _vk;

        public MessagesCategory(VkApi vk)
        {
            _vk = vk;
        }

        /// <summary>
        /// Возвращает список входящих либо исходящих личных сообщений текущего пользователя. 
        /// </summary>
        /// <param name="type">Тип сообщений которые необходимо получить.
        /// Необходимо передать <see cref="MessageType.Received"/> для полученных сообщений и <see cref="MessageType.Sended"/>
        /// для отправленных пользователем сообщений.
        /// </param>
        /// <param name="totalCount">Общее количество сообщений, удовлетворяющих условиям фильтрации.</param>
        /// <param name="count">Количество сообщений, которое необходимо получить (но не более 100).</param>
        /// <param name="offset">Смещение, необходимое для выборки определенного подмножества сообщений.</param>
        /// <param name="filter">Фильтр возвращаемых сообщений: Если установлен флаг <see cref="MessagesFilter.FromFriends"/>, то 
        /// флаги <see cref="MessagesFilter.Unread"/> и <see cref="MessagesFilter.NotFromChat"/> не учитываются.</param>
        /// <param name="previewLength">Количество символов, по которому нужно обрезать сообщение. 
        /// Укажите 0, если Вы не хотите обрезать сообщение. (по умолчанию сообщения не обрезаются). 
        /// Обратите внимание что сообщения обрезаются по словам.</param>
        /// <param name="startDate">Время, начиная с которого необходимо вернуть сообщения.</param>
        /// <returns>Список сообщений, удовлетворяющий условиям фильтрации.</returns>
        /// <remarks>
        /// Для вызова этого метода Ваше приложение должно иметь права с битовой маской, содержащей <see cref="Settings.Messages"/>. 
        /// Страница документации ВКонтакте <see cref="http://vk.com/dev/messages.get"/>.
        /// </remarks>
        public List<Message> Get(
            MessageType type,
            out int totalCount,
            int? count = null,
            int? offset = null,
            MessagesFilter? filter = null,
            int? previewLength = null,
            DateTime? startDate = null)
        {
            var parameters = new VkParameters
                             {
                                 { "out", type },
                                 { "offset", offset },
                                 { "count", count },
                                 { "filters", filter },
                                 { "preview_length", previewLength },
                                 { "time_offset", startDate }
                             };

            VkResponseArray response = _vk.Call("messages.get", parameters);

            totalCount = response[0];

            return response.Skip(1).ToListOf(r => (Message)r);
        }

        /// <summary>
        /// Возвращает историю сообщений текущего пользователя с указанным пользователя или групповой беседы. 
        /// </summary>
        /// <param name="id">
        /// Если параметр <see cref="isChat"/> равен false, то задает идентификатор пользователя, историю переписки 
        /// с которым необходимо вернуть.
        /// Если параметр <see cref="isChat"/> равен true, то задает идентификатор беседы, историю переписки в которой 
        /// необходимо вернуть.
        /// </param>
        /// <param name="isChat">Признак нужно ли вернуть историю сообщений для беседы (true) или для указанного пользователя.</param>
        /// <param name="totalCount">Общее количество сообщений в истории.</param>
        /// <param name="offset">Смещение, необходимое для выборки определенного подмножества сообщений.</param>
        /// <param name="count">Количество сообщений, которое необходимо получить (но не более 200).</param>
        /// <param name="inReverse">
        /// Если данный параметр равен true, то сообщения возвращаются в хронологическом порядке. 
        /// Если данный параметр равен false (по умолчанию), сообщения возвращаются в обратном хронологическом порядке. 
        /// </param>
        /// <param name="startMessageId">Идентификатор сообщения, начиная с которго необходимо получить последующие сообщения.</param>
        /// <returns>
        /// Запрошенные сообщения.
        /// </returns>
        /// <remarks>
        /// Для вызова этого метода Ваше приложение должно иметь права с битовой маской, содержащей <see cref="Settings.Messages"/>. 
        /// Страница документации ВКонтакте <see cref="http://vk.com/dev/messages.getHistory"/>.
        /// </remarks>
        public List<Message> GetHistory(
            long id,
            bool isChat,
            out int totalCount,
            int? offset = null,
            int? count = null,
            bool? inReverse = null,
            long? startMessageId = null)
        {
            var parameters = new VkParameters
                             {
                                 { isChat ? "chat_id" : "uid", id },
                                 { "offset", offset },
                                 { "count", count },
                                 { "start_mid", startMessageId },
                                 { "rev", inReverse }
                             };

            VkResponseArray response = _vk.Call("messages.getHistory", parameters);

            totalCount = response[0];

            return response.Skip(1).ToListOf(r => (Message)r);
        }

        /// <summary>
        /// Возвращает сообщения по их идентификаторам.
        /// </summary>
        /// <param name="messageIds">Идентификаторы сообщений, которые необходимо вернуть (не более 100).</param>
        /// <param name="totalCount">Общее количество сообщений.</param>
        /// <param name="previewLength">Количество слов, по которому нужно обрезать сообщение. 
        /// Укажите 0, если Вы не хотите обрезать сообщение. (по умолчанию сообщения не обрезаются).</param>
        /// <returns>Запрошенные сообщения.</returns>
        /// <remarks>
        /// Для вызова этого метода Ваше приложение должно иметь права с битовой маской, содержащей <see cref="Settings.Messages"/>. 
        /// Страница документации ВКонтакте <see cref="http://vk.com/dev/messages.getById"/>.
        /// </remarks>
        public List<Message> GetById(IEnumerable<long> messageIds, out int totalCount, int? previewLength = null)
        {
            var parameters = new VkParameters { { "mids", messageIds }, { "preview_length", previewLength } };

            VkResponseArray response = _vk.Call("messages.getById", parameters);

            totalCount = response[0];

            return response.Skip(1).ToListOf(r => (Message)r);
        }

        /// <summary>
        /// Ворзвращает указанное сообщение по его идентификаторы.
        /// </summary>
        /// <param name="messageId">Идентификатор запрошенного сообщения.</param>
        /// <param name="previewLength">Количество слов, по которому нужно обрезать сообщение. 
        /// Укажите 0, если Вы не хотите обрезать сообщение. (по умолчанию сообщения не обрезаются).</param>
        /// <returns>Запрошенное сообщение, null если сообщение с заданным идентификатором не найдено.</returns>
        /// <remarks>
        /// Для вызова этого метода Ваше приложение должно иметь права с битовой маской, содержащей <see cref="Settings.Messages"/>. 
        /// Страница документации ВКонтакте <see cref="http://vk.com/dev/messages.getById"/>.
        /// </remarks>
        public Message GetById(long messageId, int? previewLength = null)
        {
            int totalCount;

            return GetById(new[] { messageId }, out totalCount, previewLength).FirstOrDefault();
        }

        /// <summary>
        /// Возвращает список диалогов текущего пользователя.
        /// </summary>
        /// <param name="userId">Идентификатор пользователя, последнее сообщение в переписке с которым необходимо вернуть.</param>
        /// <param name="totalCount">Общее количество диалогов с учетом фильтра.</param>
        /// <param name="chatId">Идентификатор беседы, последнее сообщение в которой необходимо вернуть.</param>
        /// <param name="count">Количество диалогов, которое необходимо получить (но не более 200).</param>
        /// <param name="offset">Смещение, необходимое для выборки определенного подмножества диалогов.</param>
        /// <param name="previewLength">Количество символов, по которому нужно обрезать сообщение. 
        /// Укажите 0, если Вы не хотите обрезать сообщение. (по умолчанию сообщения не обрезаются).</param>
        /// <returns>Список диалогов текущего пользователя.</returns>
        /// <remarks>
        /// Для вызова этого метода Ваше приложение должно иметь права с битовой маской, содержащей <see cref="Settings.Messages"/>. 
        /// Страница документации ВКонтакте <see cref="http://vk.com/dev/messages.getDialogs"/>.
        /// </remarks>
        public List<Message> GetDialogs(long userId, out int totalCount, long? chatId = null, int? count = null, int? offset = null, int? previewLength = null)
        {
            var parameters = new VkParameters { { "uid", userId }, { "chat_id", chatId }, { "count", count }, { "offset", offset }, { "preview_length", previewLength } };

            VkResponseArray response = _vk.Call("messages.getDialogs", parameters);

            totalCount = response[0];

            return response.Skip(1).ToListOf(r => (Message)r);
        }

        /// <summary>
        /// Возвращает список найденных диалогов текущего пользователя по введенной строке поиска. 
        /// </summary>
        /// <param name="query">Подстрока, по которой будет производиться поиск.</param>
        /// <param name="fields">Поля профилей собеседников, которые необходимо вернуть.</param>
        /// <returns>
        /// В результате выполнения данного метода будет возвращён массив объектов профилей, бесед и email.
        /// </returns>
        /// <remarks>
        /// Для вызова этого метода Ваше приложение должно иметь права с битовой маской, содержащей <see cref="Settings.Messages"/>. 
        /// Страница документации ВКонтакте <see cref="http://vk.com/dev/messages.searchDialogs"/>.
        /// </remarks>
        public SearchDialogsResponse SearchDialogs(string query, ProfileFields fields = null)
        {
            if (string.IsNullOrEmpty(query))
                throw new ArgumentException("Query can not be null or empty.", "query");

            var parameters = new VkParameters { { "q", query }, { "fields", fields } };

            VkResponseArray response = _vk.Call("messages.searchDialogs", parameters);

            var result = new SearchDialogsResponse();
            foreach (var record in response)
            {
                string type = record["type"];
                if (type == "profile")
                    result.Users.Add(record);
                else if (type == "chat")
                    result.Chats.Add(record);
                // TODO: Add email support.
            }
            return result;
        }

        /// <summary>
        /// Возвращает список найденных личных сообщений текущего пользователя по введенной строке поиска. 
        /// </summary>
        /// <param name="query">Подстрока, по которой будет производиться поиск.</param>
        /// <param name="totalCount">Общее количество найденных сообщений.</param>
        /// <param name="count">Количество сообщений, которое необходимо получить (но не более 100).</param>
        /// <param name="offset">Смещение, необходимое для выборки определенного подмножества сообщений из списка найденных.</param>
        /// <returns>Список личных сообщений пользователя, удовлетворяющих условиям запроса.</returns>
        /// <remarks>
        /// Для вызова этого метода Ваше приложение должно иметь права с битовой маской, содержащей <see cref="Settings.Messages"/>. 
        /// Страница документации ВКонтакте <see cref="http://vk.com/dev/messages.search"/>.
        /// </remarks>
        public List<Message> Search(string query, out int totalCount, int? count = null, int? offset = null)
        {
            if (string.IsNullOrEmpty(query))
                throw new ArgumentException("Query can not be null or empty.", "query");

            var parameters = new VkParameters { { "q", query }, { "count", count }, { "offset", offset } };

            VkResponseArray response = _vk.Call("messages.search", parameters);

            totalCount = response[0];

            return response.Skip(1).ToListOf(r => (Message)r);
        }

        /// <summary>
        /// Посылает личное сообщение.
        /// </summary>
        /// <param name="id">
        /// Если параметр <see cref="isChat"/> равен false, то задает идентификатор пользователя, которому необходимо послать сообщение.
        /// Если параметр <see cref="isChat"/> равен true, то задает идентификатор беседы, к которой будет относиться сообщение.
        /// </param>
        /// <param name="isChat">Признак посылается ли сообщение в беседу (true) или указанному пользователю (false).</param>
        /// <param name="message">Текст личного cообщения (является обязательным, если не задан параметр <see cref="attachment"/>).</param>
        /// <param name="title">Заголовок сообщения.</param>
        /// <param name="attachment">Медиа-приложение к личному сообщению.</param>
        /// <param name="forwardMessagedIds">Идентификаторы пересылаемых сообщений. Перечисленные сообщения отправителя будут отображаться 
        /// в теле письма у получателя.</param>
        /// <param name="fromChat">Задайте false для обычного сообщения и true для сообщения из часта.</param>
        /// <param name="latitude">Широта при добавлении местоположения.</param>
        /// <param name="longitude">Долгота при добавлении местоположения.</param>
        /// <param name="guid">Уникальный строковой идентификатор, предназначенный для предотвращения повторной отправки одинакового сообщения.</param>
        /// <returns>Возвращается идентификатор отправленного сообщения.</returns>
        /// <remarks>
        /// Для вызова этого метода Ваше приложение должно иметь права с битовой маской, содержащей <see cref="Settings.Messages"/>. 
        /// Страница документации ВКонтакте <see cref="http://vk.com/dev/messages.send"/>.
        /// </remarks>
        public long Send(
            long id,
            bool isChat,
            string message,
            string title = "",
            Attachment attachment = null,
            IEnumerable<long> forwardMessagedIds = null,
            bool fromChat = false,
            double? latitude = null,
            double? longitude = null,
            string guid = null)
        {
            if (string.IsNullOrEmpty(message))
                throw new ArgumentException("Message can not be null.", "message");

            var parameters = new VkParameters
                             {
                                 { isChat ? "chat_id" : "uid", id },
                                 { "message", HttpUtility.UrlEncode(message) },
                                 { "forward_messages", forwardMessagedIds },
                                 { "title", HttpUtility.UrlEncode(title) },
                                 { "type", fromChat },
                                 { "lat", latitude },
                                 { "long", longitude },
                                 { "guid", HttpUtility.UrlEncode(guid) }
                             };

            // TODO: Yet not work with attachments. Fix it later.

            return _vk.Call("messages.send", parameters);
        }

        /// <summary>
        /// Удаляет все личные сообщения в диалоге. 
        /// </summary>
        /// <param name="id">
        /// Если параметр <see cref="isChat"/> равен false, то задает идентификатор пользователя, из диалога с которым необходимо удалить свои личные сообщения.
        /// Если параметр <see cref="isChat"/> равен true, то задает идентификатор беседы, из которой необходимо удалить свои личные сообщения.
        /// </param>
        /// <param name="isChat">Признак удаляются ли сообщения из беседы (true) или из диалога с указанным пользователем (false).</param>
        /// <param name="offset">Смещение, начиная с которого нужно удалить переписку (по умолчанию удаляются все сообщения,
        ///  начиная с первого).</param>
        /// <param name="limit">Как много сообщений нужно удалить. Обратите внимание что на метод наложено ограничение, за один вызов 
        /// нельзя удалить больше 10000 сообщений, поэтому если сообщений в переписке больше - метод нужно вызывать несколько раз.</param>
        /// <returns>Признак удалось ли удалить сообщения.</returns>
        /// <remarks>
        /// Для вызова этого метода Ваше приложение должно иметь права с битовой маской, содержащей <see cref="Settings.Messages"/>. 
        /// Страница документации ВКонтакте <see cref="http://vk.com/dev/messages.deleteDialog"/>.
        /// </remarks>
        public bool DeleteDialog(long id, bool isChat, int? offset = null, int? limit = null)
        {
            var parameters = new VkParameters { { isChat ? "chat_id" : "uid", id }, { "offset", offset }, { "limit", limit } };

            return _vk.Call("messages.deleteDialog", parameters);
        }

        /// <summary>
        /// Удаляет сообщения пользователя.
        /// </summary>
        /// <param name="messageIds">
        /// Идентификаторы удаляемых сообщений.
        /// </param>
        /// <returns>
        /// Возвращает словарь (идентификатор сообщения -> признак было ли удаление сообщения успешным).
        /// </returns>
        /// <remarks>
        /// Для вызова этого метода Ваше приложение должно иметь права с битовой маской, содержащей <see cref="Settings.Messages"/>. 
        /// Страница документации ВКонтакте <see cref="http://vk.com/dev/messages.delete"/>.
        /// </remarks>
        public IDictionary<long, bool> Delete(IEnumerable<long> messageIds)
        {
            if (messageIds == null)
                throw new ArgumentNullException("messageIds", "Parameter messageIds can not be null.");

            var ids = messageIds.ToList();
            if (ids.Count == 0)
                throw new ArgumentException("Parameter messageIds has no elements.", "messageIds");

            var parameters = new VkParameters { { "mids", ids } };

            var response = _vk.Call("messages.delete", parameters);

            var result = new Dictionary<long, bool>();
            foreach (var id in ids)
            {
                bool isDeleted = response[id.ToString(CultureInfo.InvariantCulture)];
                result.Add(id, isDeleted);
            }

            return result;
        }

        /// <summary>
        /// Удаляет личное сообщение пользователя с заданным идентификатором.
        /// </summary>
        /// <param name="messageId">
        /// Идентификатор удаляемого сообщения.
        /// </param>
        /// <returns>
        /// Признак было ли удаление сообщения успешным.
        /// </returns>
        /// <remarks>
        /// Для вызова этого метода Ваше приложение должно иметь права с битовой маской, содержащей <see cref="Settings.Messages"/>. 
        /// Страница документации ВКонтакте <see cref="http://vk.com/dev/messages.delete"/>.
        /// </remarks>
        public bool Delete(long messageId)
        {
            var result = Delete(new[] { messageId });
            return result[messageId];
        }

        /// <summary>
        /// Восстанавливает удаленное сообщение.
        /// </summary>
        /// <param name="messageId">Идентификатор сообщения, которое нужно восстановить.</param>
        /// <remarks>
        /// Для вызова этого метода Ваше приложение должно иметь права с битовой маской, содержащей <see cref="Settings.Messages"/>. 
        /// Страница документации ВКонтакте <see cref="http://vk.com/dev/messages.restore"/>.
        /// </remarks>
        public bool Restore(long messageId)
        {
            var parameters = new VkParameters { { "message_id", messageId } };

            return _vk.Call("messages.restore", parameters);
        }

        /// <summary>
        /// Помечает сообщения как непрочитанные. 
        /// </summary>
        /// <param name="messageIds">
        /// Идентификаторы сообщений, которые нужно пометить как непрочитанные. 
        /// </param>
        /// <returns>
        /// Возвращает true, если сообщения были успешно помечены как непрочитанные, false в противном случае.
        /// </returns>
        /// <remarks>
        /// Для вызова этого метода Ваше приложение должно иметь права с битовой маской, содержащей <see cref="Settings.Messages"/>. 
        /// Страница документации ВКонтакте <see cref="http://vk.com/dev/messages.markAsNew"/>.
        /// </remarks>
        public bool MarkAsNew(IEnumerable<long> messageIds)
        {
            var parameters = new VkParameters { { "mids", messageIds } };

            return _vk.Call("messages.markAsNew", parameters);
        }

        /// <summary>
        /// Помечает указанное сообщение как непрочитанное.
        /// </summary>
        /// <param name="messageId">
        /// Идентификатор сообщения, которое необходимо пометить как прочитанное.
        /// </param>
        /// <returns>
        /// Возвращает true, если сообщение были успешно помечено как непрочитанное, false в противном случае.
        /// </returns>
        /// <remarks>
        /// Для вызова этого метода Ваше приложение должно иметь права с битовой маской, содержащей <see cref="Settings.Messages"/>. 
        /// Страница документации ВКонтакте <see cref="http://vk.com/dev/messages.markAsNew"/>.
        /// </remarks>
        public bool MarkAsNew(long messageId)
        {
            return MarkAsNew(new[] { messageId });
        }

        /// <summary>
        /// Помечает сообщения как прочитанные. 
        /// </summary>
        /// <param name="messageIds">
        /// Идентификаторы сообщений, которые нужно пометить как прочитанные. 
        /// </param>
        /// <returns>
        /// Возвращает true, если сообщения были успешно помечены как прочитанные, false в противном случае.
        /// </returns>
        /// <remarks>
        /// Для вызова этого метода Ваше приложение должно иметь права с битовой маской, содержащей <see cref="Settings.Messages"/>. 
        /// Страница документации ВКонтакте <see cref="http://vk.com/dev/messages.markAsRead"/>.
        /// </remarks>
        public bool MarkAsRead(IEnumerable<long> messageIds)
        {
            var parameters = new VkParameters { { "mids", messageIds } };

            return _vk.Call("messages.markAsRead", parameters);
        }

        /// <summary>
        /// Помечает сообщение как прочитанное.
        /// </summary>
        /// <param name="messageId">
        /// Идентификатор сообщения, которое необходимо пометить как прочитанное.
        /// </param>
        /// <returns>
        /// Возвращает true, если сообщение были успешно помечено как прочитанное, false в противном случае.
        /// </returns>
        /// <remarks>
        /// Для вызова этого метода Ваше приложение должно иметь права с битовой маской, содержащей <see cref="Settings.Messages"/>. 
        /// Страница документации ВКонтакте <see cref="http://vk.com/dev/messages.markAsRead"/>.
        /// </remarks>
        public bool MarkAsRead(long messageId)
        {
            return MarkAsRead(new[] { messageId });
        }

        /// <summary>
        /// Изменяет статус набора текста пользователем в диалоге.
        /// </summary>
        /// <param name="userId">
        /// Идентификатор пользователя
        /// </param>
        /// <returns>
        /// После успешного выполнения возвращает true, false в противном случае. 
        /// Текст «N набирает сообщение...» отображается в течение 10 секунд после вызова метода, либо до момента отправки сообщения. 
        /// </returns>
        /// <remarks>
        /// Для вызова этого метода Ваше приложение должно иметь права с битовой маской, содержащей <see cref="Settings.Messages"/>. 
        /// Страница документации ВКонтакте <see cref="http://vk.com/dev/messages.setActivity"/>.
        /// </remarks>
        public bool SetActivity(long userId)
        {
            var parameters = new VkParameters { { "used_id", userId }, { "type", "typing" } };

            return _vk.Call("messages.setActivity", parameters);
        }

        /// <summary>
        /// Возвращает текущий статус и дату последней активности указанного пользователя. 
        /// </summary>
        /// <param name="userId">
        /// Идентификатор пользователя, информацию о последней активности которого требуется получить. 
        /// </param>
        /// <returns>
        /// Возвращает объект, содержащий информацию об активности пользователя.
        /// </returns>
        /// <remarks>
        /// Для вызова этого метода Ваше приложение должно иметь права с битовой маской, содержащей <see cref="Settings.Messages"/>. 
        /// Страница документации ВКонтакте <see cref="http://vk.com/dev/messages.getLastActivity"/>.
        /// </remarks>
        public LastActivity GetLastActivity(long userId)
        {
            var parameters = new VkParameters { { "user_id", userId } };

            var response = _vk.Call("messages.getLastActivity", parameters);

            LastActivity activity = response;
            activity.UserId = userId;

            return activity;
        }

        /// <summary>
        /// Возвращает информацию о беседе. 
        /// </summary>
        /// <param name="chatId">
        /// Идентификатор беседы.
        /// </param>
        /// <returns>
        /// После успешного выполнения возварщает список объектов, описывающих беседу (мультидиалог).
        /// </returns>
        /// <remarks>
        /// Для вызова этого метода Ваше приложение должно иметь права с битовой маской, содержащей <see cref="Settings.Messages"/>. 
        /// Страница документации ВКонтакте <see cref="http://vk.com/dev/messages.getChat"/>.
        /// </remarks>
        public Chat GetChat(long chatId)
        {
            var parameters = new VkParameters { { "chat_id", chatId } };

            var response = _vk.Call("messages.getChat", parameters);

            return response;
        }

        /// <summary>
        /// Создаёт беседу с несколькими участниками. 
        /// </summary>
        /// <param name="userIds">Идентификаторы пользователей, которых нужно включить в беседу (мультидиалог).</param>
        /// <param name="title">Название беседы.</param>
        /// <returns>После успешного выполнения возвращает идентификатор созданной беседы.</returns>
        /// <remarks>
        /// Для вызова этого метода Ваше приложение должно иметь права с битовой маской, содержащей <see cref="Settings.Messages"/>. 
        /// Страница документации ВКонтакте <see cref="http://vk.com/dev/messages.createChat"/>.
        /// </remarks>
        public long CreateChat(IEnumerable<long> userIds, string title)
        {
            if (string.IsNullOrEmpty(title))
                throw new ArgumentException("Title can not be empty or null.", "userIds");

            var parameters = new VkParameters { { "uids", userIds }, { "title", HttpUtility.UrlEncode(title) } };

            return _vk.Call("messages.createChat", parameters);
        }

        /// <summary>
        /// Изменяет название беседы.
        /// </summary>
        /// <param name="chatId">Идентификатор беседы.</param>
        /// <param name="title">Новое название для беседы.</param>
        /// <returns>
        /// После успешного выполнения возвращает true, false в противном случае. 
        /// </returns>
        /// <remarks>
        /// Для вызова этого метода Ваше приложение должно иметь права с битовой маской, содержащей <see cref="Settings.Messages"/>. 
        /// Страница документации ВКонтакте <see cref="http://vk.com/dev/messages.editChat"/>.
        /// </remarks>
        public bool EditChat(long chatId, string title)
        {
            if (string.IsNullOrEmpty(title))
                throw new ArgumentException("Title can not be empty or null.", "title");

            var parameters = new VkParameters { { "chat_id", chatId }, { "title", HttpUtility.UrlEncode(title) } };

            return _vk.Call("messages.editChat", parameters);
        }

        /// <summary>
        /// Позволяет получить список пользователей беседы по ее идентификатору. 
        /// </summary>
        /// <param name="chatId">Идентификатор беседы.</param>
        /// <param name="fields">Список дополнительных полей профилей, которые необходимо вернуть.</param>
        /// <returns>После успешного выполнения возвращает список идентификаторов участников беседы.</returns>
        /// <remarks>
        /// Для вызова этого метода Ваше приложение должно иметь права с битовой маской, содержащей <see cref="Settings.Messages"/>. 
        /// Страница документации ВКонтакте <see cref="http://vk.com/dev/messages.getChatUsers"/>.
        /// </remarks>
        public List<User> GetChatUsers(long chatId, ProfileFields fields)
        {
            var parameters = new VkParameters { { "chat_id", chatId }, { "fields", fields } };

            var response = _vk.Call("messages.getChatUsers", parameters);

            if (fields != null)
                return response;

            return response.ToListOf(x => new User { Id = (long)x });
        }

        /// <summary>
        /// Позволяет получить список пользователей беседы по ее идентификатору. 
        /// </summary>
        /// <param name="chatId">Идентификатор беседы.</param>
        /// <returns>После успешного выполнения возвращает список идентификаторов участников беседы.</returns>
        /// <remarks>
        /// Для вызова этого метода Ваше приложение должно иметь права с битовой маской, содержащей <see cref="Settings.Messages"/>. 
        /// Страница документации ВКонтакте <see cref="http://vk.com/dev/messages.getChatUsers"/>.
        /// </remarks>
        public List<long> GetChatUsers(long chatId)
        {
            var users = GetChatUsers(chatId, null);
            return users.Select(x => x.Id).ToList();
        }

        /// <summary>
        /// Добавляет в беседу нового пользователя. 
        /// </summary>
        /// <param name="chatId">Идентификатор беседы.</param>
        /// <param name="userId">Идентификатор пользователя, которого необходимо включить в беседу.</param>
        /// <returns>
        /// После успешного выполнения возвращает true, false в противном случае. 
        /// </returns>
        /// <remarks>
        /// Для вызова этого метода Ваше приложение должно иметь права с битовой маской, содержащей <see cref="Settings.Messages"/>. 
        /// Страница документации ВКонтакте <see cref="http://vk.com/dev/messages.addChatUser"/>.
        /// </remarks>
        public bool AddChatUser(long chatId, long userId)
        {
            var parameters = new VkParameters { { "chat_id", chatId }, { "uid", userId } };

            return _vk.Call("messages.addChatUser", parameters);
        }

        /// <summary>
        /// Исключает из беседы пользователя, если текущий пользователь был создателем беседы либо пригласил исключаемого пользователя. 
        /// </summary>
        /// <param name="chatId">
        /// Идентификатор беседы.
        /// </param>
        /// <param name="userId">
        /// Идентификатор пользователя, которого необходимо исключить из беседы. 
        /// </param>
        /// <returns>
        /// После успешного выполнения возвращает true, false в противном случае. 
        /// </returns>
        /// <remarks>
        /// Для вызова этого метода Ваше приложение должно иметь права с битовой маской, содержащей <see cref="Settings.Messages"/>. 
        /// Страница документации ВКонтакте <see cref="http://vk.com/dev/messages.removeChatUser"/>.
        /// </remarks>
        public bool RemoveChatUser(long chatId, long userId)
        {
            var parameters = new VkParameters { { "chat_id", chatId }, { "uid", userId } };

            return _vk.Call("messages.removeChatUser", parameters);
        }

        /// <summary>
        /// Возвращает данные, необходимые для подключения к Long Poll серверу. 
        /// Long Poll подключение позволит Вам моментально узнавать о приходе новых сообщений и других событий. 
        /// </summary>
        /// <returns>
        /// Возвращает объект, с помощью которого можно подключиться к серверу быстрых сообщений для мгновенного 
        /// получения приходящих сообщений и других событий.  
        /// </returns>
        /// <remarks>
        /// Для вызова этого метода Ваше приложение должно иметь права с битовой маской, содержащей <see cref="Settings.Messages"/>. 
        /// Страница документации ВКонтакте <see cref="http://vk.com/dev/messages.getLongPollServer"/>.
        /// </remarks>
        public LongPollServerResponse GetLongPollServer()
        {
            return _vk.Call("messages.getLongPollServer", VkParameters.Empty);
        }

        /// <summary>
        /// Возвращает обновления в личных сообщениях пользователя. 
        /// Для ускорения работы с личными сообщениями может быть полезно кешировать уже загруженные ранее сообщения на 
        /// мобильном устройстве / ПК пользователя, чтобы не получать их повторно при каждом обращении. 
        /// Этот метод помогает осуществить синхронизацию локальной копии списка сообщений с актуальной версией. 
        /// </summary>
        /// <remarks>
        /// Для вызова этого метода Ваше приложение должно иметь права с битовой маской, содержащей <see cref="Settings.Messages"/>. 
        /// Страница документации ВКонтакте <see cref="http://vk.com/dev/messages.getLongPollHistory"/>.
        /// </remarks>
        internal void GetLongPollHistory()
        {
            // TODO: Implement later
            throw new NotImplementedException();
        }
    }
}