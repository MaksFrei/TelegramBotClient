﻿//------------------------------------------------------------------------------
// <auto-generated>
//     Этот код создан программой.
//     Исполняемая версия:4.0.30319.42000
//
//     Изменения в этом файле могут привести к неправильной работе и будут потеряны в случае
//     повторной генерации кода.
// </auto-generated>
//------------------------------------------------------------------------------

namespace BotClient.Properties {
    using System;
    
    
    /// <summary>
    ///   Класс ресурса со строгой типизацией для поиска локализованных строк и т.д.
    /// </summary>
    // Этот класс создан автоматически классом StronglyTypedResourceBuilder
    // с помощью такого средства, как ResGen или Visual Studio.
    // Чтобы добавить или удалить член, измените файл .ResX и снова запустите ResGen
    // с параметром /str или перестройте свой проект VS.
    [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Resources.Tools.StronglyTypedResourceBuilder", "16.0.0.0")]
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
    [global::System.Runtime.CompilerServices.CompilerGeneratedAttribute()]
    internal class Resources {
        
        private static global::System.Resources.ResourceManager resourceMan;
        
        private static global::System.Globalization.CultureInfo resourceCulture;
        
        [global::System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
        internal Resources() {
        }
        
        /// <summary>
        ///   Возвращает кэшированный экземпляр ResourceManager, использованный этим классом.
        /// </summary>
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Advanced)]
        internal static global::System.Resources.ResourceManager ResourceManager {
            get {
                if (object.ReferenceEquals(resourceMan, null)) {
                    global::System.Resources.ResourceManager temp = new global::System.Resources.ResourceManager("BotClient.Properties.Resources", typeof(Resources).Assembly);
                    resourceMan = temp;
                }
                return resourceMan;
            }
        }
        
        /// <summary>
        ///   Перезаписывает свойство CurrentUICulture текущего потока для всех
        ///   обращений к ресурсу с помощью этого класса ресурса со строгой типизацией.
        /// </summary>
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Advanced)]
        internal static global::System.Globalization.CultureInfo Culture {
            get {
                return resourceCulture;
            }
            set {
                resourceCulture = value;
            }
        }
        
        /// <summary>
        ///   Ищет локализованную строку, похожую на CREATE TABLE &quot;Chats&quot; (
        ///	&quot;IdChat&quot;	INT NOT NULL,
        ///	&quot;Name&quot;	TEXT,
        ///	PRIMARY KEY(&quot;IdChat&quot;)
        ///);
        ///
        ///CREATE TABLE &quot;Messages&quot; (
        ///	&quot;IdMessage&quot;	integer PRIMARY KEY AUTOINCREMENT,
        ///	&quot;Text&quot;	TEXT,
        ///	&quot;IdChat&quot;	INT,
        ///	&quot;IdUser&quot;	INT,
        ///	FOREIGN KEY(&quot;IdChat&quot;) REFERENCES &quot;Chats&quot;,
        ///	FOREIGN KEY(&quot;IdUser&quot;) REFERENCES &quot;Users&quot;
        ///);
        ///
        ///CREATE TABLE &quot;Users&quot; (
        ///	&quot;IdUser&quot;	INT NOT NULL,
        ///	&quot;LastName&quot;	TEXT,
        ///	&quot;FirstName&quot;	TEXT,
        ///	&quot;NickName&quot;	TEXT,
        ///	PRIMARY KEY(&quot;IdUser&quot;)
        ///);.
        /// </summary>
        internal static string DBSchema {
            get {
                return ResourceManager.GetString("DBSchema", resourceCulture);
            }
        }
    }
}
