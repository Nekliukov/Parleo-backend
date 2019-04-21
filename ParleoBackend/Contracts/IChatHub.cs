using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ParleoBackend.ViewModels.Entities;

namespace ParleoBackend.Contracts
{
    public interface IChatHub
    {
         Task SubscribeToChat(Guid chatId);

         Task SubscribeToChats(ICollection<Guid> chatIds);

         Task SendMessage(MessageViewModel message);


    }
}
