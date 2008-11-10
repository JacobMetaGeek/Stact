// Copyright 2007-2008 The Apache Software Foundation.
//  
// Licensed under the Apache License, Version 2.0 (the "License"); you may not use 
// this file except in compliance with the License. You may obtain a copy of the 
// License at 
// 
//     http://www.apache.org/licenses/LICENSE-2.0 
// 
// Unless required by applicable law or agreed to in writing, software distributed 
// under the License is distributed on an "AS IS" BASIS, WITHOUT WARRANTIES OR 
// CONDITIONS OF ANY KIND, either express or implied. See the License for the 
// specific language governing permissions and limitations under the License.
namespace Magnum.ProtocolBuffers.Specs.Serialization
{
    using System;
    using NUnit.Framework;
    using ProtocolBuffers.Serialization;

    [TestFixture]
    public class When_writing_to_a_CodedOutputStream
    {
        [Test]
        public void An_Int32_should_be_stored()
        {
            var outputStream = new CodedOutputStream();

            var message = new Int32Message(150);

            var map = new Int32MessageMap();

            var descriptor = new MessageDescriptor<Int32Message>();

            descriptor.Serialize(outputStream, message);

            outputStream.WriteInt32(1, message.Value);

            int finalLength = outputStream.Length;

            Assert.AreEqual(3, finalLength);

            byte[] block = outputStream.GetBytes();
            byte[] expected = new byte[] { 0x08, 0x96, 0x01 };

            Assert.AreEqual(expected, block);
        }

        [Test]
        public void A_string_should_be_properly_encoded()
        {
            var outputStream = new CodedOutputStream();

            string value = "testing";

            outputStream.WriteString(2, value);

            byte[] block = outputStream.GetBytes();

            byte[] expected = new byte[] { 0x12, 0x07, 0x74, 0x65, 0x73, 0x74, 0x69, 0x6e, 0x67 };

            Assert.AreEqual(expected, block);
        }
    }

    public class Int32MessageMap :
        MessageMap<Int32Message>
    {
        public Int32MessageMap()
        {
            Field(x => x.Value).MakeRequired();
        }
    }

    public class Test3Map :
        MessageMap<Test3>
    {
        public Test3Map()
        {
            Field(m => m.I);
        }
    }

    //test1
    public class Int32Message
    {
        public Int32Message(Int32 value)
        {
            Value = value;
        }

        public int Value { get; set; }
    }

    public class Test3
    {
        public Int32Message I { get; set; }
    }
}