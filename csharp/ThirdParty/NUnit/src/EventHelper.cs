#region Copyright (c) 2003-2007, Luke T. Maxon

/********************************************************************************************************************
'
' Copyright (c) 2003-2007, Luke T. Maxon
' All rights reserved.
' 
' Redistribution and use in source and binary forms, with or without modification, are permitted provided
' that the following conditions are met:
' 
' * Redistributions of source code must retain the above copyright notice, this list of conditions and the
' 	following disclaimer.
' 
' * Redistributions in binary form must reproduce the above copyright notice, this list of conditions and
' 	the following disclaimer in the documentation and/or other materials provided with the distribution.
' 
' * Neither the name of the author nor the names of its contributors may be used to endorse or 
' 	promote products derived from this software without specific prior written permission.
' 
' THIS SOFTWARE IS PROVIDED BY THE COPYRIGHT HOLDERS AND CONTRIBUTORS "AS IS" AND ANY EXPRESS OR IMPLIED
' WARRANTIES, INCLUDING, BUT NOT LIMITED TO, THE IMPLIED WARRANTIES OF MERCHANTABILITY AND FITNESS FOR A
' PARTICULAR PURPOSE ARE DISCLAIMED. IN NO EVENT SHALL THE COPYRIGHT OWNER OR CONTRIBUTORS BE LIABLE FOR
' ANY DIRECT, INDIRECT, INCIDENTAL, SPECIAL, EXEMPLARY, OR CONSEQUENTIAL DAMAGES (INCLUDING, BUT NOT
' LIMITED TO, PROCUREMENT OF SUBSTITUTE GOODS OR SERVICES; LOSS OF USE, DATA, OR PROFITS; OR BUSINESS
' INTERRUPTION) HOWEVER CAUSED AND ON ANY THEORY OF LIABILITY, WHETHER IN CONTRACT, STRICT LIABILITY,
' OR TORT (INCLUDING NEGLIGENCE OR OTHERWISE) ARISING IN ANY WAY OUT OF THE USE OF THIS SOFTWARE, EVEN
' IF ADVISED OF THE POSSIBILITY OF SUCH DAMAGE.
'
'*******************************************************************************************************************/

#endregion

        ///<summary>
        /// Fires the named event on the given object using the object's "OnEventName" method.
        ///</summary>
        /// <remarks>
        /// <para>
        /// By convention, an event named "MyEvent(object sender, MyEventArgs e)" should have a virtual protected
        /// method "OnMyEvent(MyEventArgs e)" that actually calls any attached event handler.
        /// </para>
        /// <para>
        /// This method assumes that the target event has been implemented with this pattern.
        /// </para>
        /// </remarks>
        ///<param name="targetObject">The object raising the event.</param>
        ///<param name="eventName">The name of the event to raise.</param>
        ///<param name="arg">The EventArgs-derived class to pass to this event.</param>
        public static void RaiseEvent(object targetObject, string eventName, EventArgs arg)
        {
            MethodInfo minfo = targetObject.GetType().GetMethod("On" + eventName,
                                                                BindingFlags.Instance | BindingFlags.Public |
                                                                BindingFlags.NonPublic);
            if (minfo == null)
            {
                PropertyInfo SelectionProperty = targetObject.GetType().GetProperty("Selection", BindingFlags.Instance | BindingFlags.Public |
                                                                BindingFlags.NonPublic);

                targetObject = SelectionProperty.GetGetMethod().Invoke(targetObject, null);

                minfo = targetObject.GetType().GetMethod("On" + eventName,
                                                                BindingFlags.Instance | BindingFlags.Public |
                                                                BindingFlags.NonPublic);
            }

            minfo.Invoke(targetObject, new object[] {arg});
        }
