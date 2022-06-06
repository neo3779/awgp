using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Content.Pipeline;
using Microsoft.Xna.Framework.Content.Pipeline.Graphics;
using Microsoft.Xna.Framework.Content.Pipeline.Processors;

// TODO: replace these with the processor input and output types.
using TInput = System.String;
using TOutput = AWGP.InputManager;

namespace InputManagerCPEL
{
    struct PlayerInput
    {
        AWGP.PlayersName p;
        AWGP.Controls c;
        Enum e;
        string s;

        public AWGP.PlayersName P
        {
            get { return p; }
            set { p = value; }
        }
        public AWGP.Controls C
        {
            get { return c; }
            set { c = value; }
        }
        public Enum E
        {
            get { return e; }
            set { e = value; }
        }
        public string S
        {
            get { return s; }
            set { s = value; }
        }
    }
    /// <summary>
    /// This class will be instantiated by the XNA Framework Content Pipeline
    /// to apply custom processing to content data, converting an object of
    /// type TInput to TOutput. The input and output types may be the same if
    /// the processor wishes to alter data without changing its type.
    ///
    /// This should be part of a Content Pipeline Extension Library project.
    ///
    /// TODO: change the ContentProcessor attribute to specify the correct
    /// display name for this processor.
    /// </summary>
    [ContentProcessor(DisplayName = "Input Manager Processor")]
    public class InputManagerProcessor : ContentProcessor<TInput, TOutput>
    {
        public override TOutput Process(TInput input, ContentProcessorContext context)
        {


            // TODO: process the input object, and return the modified data.
            AWGP.InputManager im;
            AWGP.Player p;
            AWGP.PlayersName name;

            List<AWGP.Player> players = new List<AWGP.Player>();
            List<AWGP.PlayersName> playerNames = new List<AWGP.PlayersName>();
            List<AWGP.Controls> controls = new List<AWGP.Controls>();
            AWGP.Controls control = AWGP.Controls.GAMEPAD;

            Enum button = AWGP.MouseMonitor.MouseButtons.LEFTBUTTON;

            List<GamePadButtons> gamePadsButtons = new List<GamePadButtons>();
            List<Keys> keys = new List<Keys>();
            List<AWGP.MouseMonitor.MouseButtons> mousebuttons = new List<AWGP.MouseMonitor.MouseButtons>();
            List<object> buttons = new List<object>();

            foreach (GamePadButtons g in gamePadsButtons)
            {
                buttons.Add(g);
            }

            foreach (Keys k in keys)
            {
                buttons.Add(k);
            }

            foreach (AWGP.MouseMonitor.MouseButtons m in mousebuttons)
            {
                buttons.Add(m);
            }



            List<AWGP.PlayerMoves> listMoves = new List<AWGP.PlayerMoves>();
            string move = "";

            List<PlayerInput> listAll = new List<PlayerInput>();
            PlayerInput pi = new PlayerInput(); ;

            string[] lines = input.Split(new char[] { '\n' });

            for (int i = 0; i < lines.Length - 1; i++)
            {
                foreach (AWGP.PlayersName pn in playerNames)
                {
                    if (0 == string.Compare(lines[i].ToString(), pn.ToString()))
                    {
                        name = pn;
                        playerNames.Remove(pn);
                    }
                    else
                    {
                        foreach (AWGP.Controls c in controls)
                        {
                            if (0 == string.Compare(lines[i].ToString(), c.ToString()))
                            {
                                control = c;
                            }
                            else
                            {
                                foreach (object o in buttons)
                                {
                                    if (0 == string.Compare(lines[i].ToString(), o.ToString()))
                                    {
                                        button = (Enum)o;
                                    }
                                    else
                                    {
                                        move = o.ToString();

                                        pi.P = pn;
                                        pi.C = control;
                                        pi.E = button;
                                        pi.S = move;
                                        listAll.Add(pi);
                                    }
                                }
                            }
                        }
                    }
                }
            }

            AWGP.PlayersName preName;
            AWGP.Controls preControl;
            PlayerInput[] cplayer = listAll.ToArray();
            List<AWGP.PlayerMoves> moves = new List<AWGP.PlayerMoves>();
            if (cplayer != null)
            {
                preName = cplayer[0].P;
                preControl = cplayer[0].C;

                foreach (PlayerInput l in listAll)
                {
                    if (preName != l.P &&
                        preControl != l.C)
                    {
                        players.Add(new AWGP.Player(preName, preControl, moves.ToArray()));
                    }
                    moves.Add(new AWGP.PlayerMoves(l.E, l.S));

                }
            }
            return im = new AWGP.InputManager(players);
        }
    }
}