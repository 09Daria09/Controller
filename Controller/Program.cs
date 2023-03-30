using DocumentFormat.OpenXml.Bibliography;
using DocumentFormat.OpenXml.Drawing.Charts;
using DocumentFormat.OpenXml.Spreadsheet;
using DocumentFormat.OpenXml.Wordprocessing;
using System;
using System.Collections.Generic;
using System.Xml.Linq;

namespace RefactoringGuru.DesignPatterns.Mediator.Conceptual
{
    public abstract class AbstractChatroom
    {
        public abstract void Register(Participant participant);
        public abstract void Send(string from, string to, string message);
    }
    public abstract class Participant
    {
        public AbstractChatroom chatroom { get; set; }
        public string Name { get; set; }

        public Participant(string name)
        {
            Name = name;
        }
        public void Send(string to, string message)
        {
            chatroom.Send(Name, to, message);
        }
        public virtual void Receive(string from, string message)
        {
            Console.WriteLine($"{from} to {Name}: {message}");
        }
        public void SetChatroom(AbstractChatroom chatroom)
        {
            this.chatroom = chatroom;
        }
    }

    public class Chatroom : AbstractChatroom
    {
        Dictionary<string, Participant> participants = new Dictionary<string, Participant>();
        public override void Register(Participant participant)
        {
            if (!participants.ContainsKey(participant.Name))
            {
                participants[participant.Name] = participant;
            }
            participant.SetChatroom(this);
        }

        public override void Send(string from, string to, string message)
        {
            Participant participant = participants[to];
            if (participant != null)
            {
                participant.Receive(from, message);
            }
        }
    }

    class NonBeatles : Participant
    {

        public NonBeatles(string name) : base(name) { }
        public override void Receive(string from, string message)
        {
            Console.WriteLine("To a non-Beatles:");
            base.Receive(from, message);
        }
    }

    public class Beatles : Participant
    {
        public Beatles(string name) : base(name) { }
        public override void Receive(string from, string message)
        {
            Console.WriteLine("To a Beatles: ");
            base.Receive(from, message);
        }
    }

    class Program
    {
        public static void Registration(AbstractChatroom chatroom, Participant[] participants)
        {
            foreach (Participant participant in participants)
            {
                chatroom.Register(participant);
            }
        }

        public static void Chat(Participant participant, string to, string message)
        {
            participant.Send(to, message);
        }
        static void Main(string[] args)
        {
            AbstractChatroom chatroom = new Chatroom();

            Participant[] participants = new Participant[]
            {
    new Beatles("George"),
    new Beatles("Paul"),
    new Beatles("Ringo"),
    new Beatles("John"),
    new NonBeatles("Yoko")
            };

            Registration(chatroom, participants);

            Chat(participants[4], "John", "Hi John!");
            Chat(participants[1], "Ringo", "All you need is love");
            Chat(participants[2], "George", "My sweet Lord");
            Chat(participants[1], "John", "Can't buy me love");
            Chat(participants[3], "Yoko", "My sweet love");



        }
    }
}
