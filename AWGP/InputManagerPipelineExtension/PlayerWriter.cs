using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content.Pipeline;
using Microsoft.Xna.Framework.Content.Pipeline.Graphics;
using Microsoft.Xna.Framework.Content.Pipeline.Processors;
using Microsoft.Xna.Framework.Content.Pipeline.Serialization.Compiler;
using AWGP;

// TODO: replace this with the type you want to write out.
using TWrite = System.String;

namespace InputManagerPipelineExtension
{
    /// <summary>
    /// This class will be instantiated by the XNA Framework Content Pipeline
    /// to write the specified data type into binary .xnb format.
    ///
    /// This should be part of a Content Pipeline Extension Library project.
    /// </summary>
    [ContentTypeWriter]
    public class PlayerWriter : ContentTypeWriter<Player>
    {
        protected override void Write(ContentWriter output, Player value)
        {
            // TODO: write the specified value to the output ContentWriter.
            //output.Write(value.playerName);
            //output.Write(value.controller);
        }

        public override string GetRuntimeReader(TargetPlatform targetPlatform)
        {
            // TODO: change this to the name of your ContentTypeReader
            // class which will be used to load this data.
            return typeof(PlayerReader).AssemblyQualifiedName;
        }
    }
}
