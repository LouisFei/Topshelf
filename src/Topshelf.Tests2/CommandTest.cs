using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using Topshelf.Hosts;
using Topshelf.Runtime;

namespace Topshelf.Tests2
{
    [TestClass]
    public class CommandTest
    {
        [TestMethod]
        public void Should_create_an_install_host()
        {
            IHost host = HostFactory.New(x =>
            {
                x.Service<MyService>();
                x.ApplyCommandLine("install");
            });

            Assert.IsInstanceOfType(host, typeof(InstallHost));
        }

        [TestMethod]
        public void Should_create_an_install_host_without_being_case_sensitive()
        {
            IHost host = HostFactory.New(x =>
            {
                x.Service<MyService>();
                x.ApplyCommandLine("Install");
            });

            Assert.IsInstanceOfType(host, typeof(InstallHost));
        }

        [TestMethod]
        public void Should_throw_an_exception_on_an_invalid_command_line()
        {
            try
            {
                HostFactory.New(x =>
                {
                    x.Service<MyService>();
                    x.ApplyCommandLine("explode");
                });
            }
            catch (HostConfigurationException exception)
            {
                Assert.IsTrue(exception.Message.Contains("explode"));
            }
        }

        [TestMethod]
        public void Should_create_an_install_host_with_service_name()
        {
            IHost host = HostFactory.New(x =>
            {
                x.Service<MyService>();
                x.ApplyCommandLine("install -servicename \"Louis\"");
            });

            Assert.IsInstanceOfType(host, typeof(InstallHost));
            var installHost = (InstallHost)host;
            Assert.AreEqual("Louis", installHost.Settings.Name);
            Assert.AreEqual("Louis", installHost.Settings.ServiceName);
        }

        [TestMethod]
        public void Should_create_an_install_host_with_service_name_no_quotes()
        {
            IHost host = HostFactory.New(x =>
            {
                x.Service<MyService>();
                x.ApplyCommandLine("install -servicename Joe");
            });

            Assert.IsInstanceOfType(host, typeof(InstallHost));
            var installHost = (InstallHost)host;
            Assert.AreEqual("Joe", installHost.Settings.Name);
            Assert.AreEqual("Joe", installHost.Settings.ServiceName);
        }

        [TestMethod]
        public void Should_create_an_install_host_with_display_name_and_instance_name()
        {
            IHost host = HostFactory.New(x =>
            {
                x.Service<MyService>();
                x.ApplyCommandLine("install -displayname \"Joe\" -instance \"42\"");
            });

            Assert.IsInstanceOfType(host, typeof(InstallHost));
            var installHost = (InstallHost)host;
            Assert.AreEqual("Joe (Instance: 42)", installHost.Settings.DisplayName);
        }

        [TestMethod]
        public void Should_create_an_install_host_with_display_name_and_instance_name_no_quotes()
        {
            IHost host = HostFactory.New(x =>
            {
                x.Service<MyService>();
                x.ApplyCommandLine("install -displayname Joe -instance 42");
            });

            Assert.IsInstanceOfType(host, typeof(InstallHost));
            var installHost = (InstallHost)host;
            Assert.AreEqual("Joe (Instance: 42)", installHost.Settings.DisplayName);
        }

        [TestMethod]
        public void Should_create_an_install_host_with_display_name_with_instance_name()
        {
            IHost host = HostFactory.New(x =>
            {
                x.Service<MyService>();
                x.ApplyCommandLine("install -displayname \"Joe (Instance: 42)\" -instance \"42\"");
            });

            Assert.IsInstanceOfType(host, typeof(InstallHost));
            var installHost = (InstallHost)host;
            Assert.AreEqual("Joe (Instance: 42)", installHost.Settings.DisplayName);
        }

        [TestMethod]
        public void Should_create_an_install_host_with_display_name_with_instance_name_no_quotes()
        {
            IHost host = HostFactory.New(x =>
            {
                x.Service<MyService>();
                x.ApplyCommandLine("install -displayname \"Joe (Instance: 42)\" -instance 42");
            });

            Assert.IsInstanceOfType(host, typeof(InstallHost));
            var installHost = (InstallHost)host;
            Assert.AreEqual("Joe (Instance: 42)", installHost.Settings.DisplayName);
        }

        [TestMethod]
        public void Should_create_an_install_host_with_description()
        {
            IHost host = HostFactory.New(x =>
            {
                x.Service<MyService>();
                x.ApplyCommandLine("install -description \"Joe is good\"");
            });

            Assert.IsInstanceOfType(host, typeof(InstallHost));
            var installHost = (InstallHost)host;
            Assert.AreEqual("Joe is good", installHost.Settings.Description);
        }

        [TestMethod]
        public void Should_create_an_install_host_with_service_name_and_instance_name()
        {
            IHost host = HostFactory.New(x =>
            {
                x.Service<MyService>();
                x.ApplyCommandLine("install -servicename \"Joe\" -instance \"42\"");
            });

            Assert.IsInstanceOfType(host, typeof(InstallHost));
            var installHost = (InstallHost)host;
            Assert.AreEqual("Joe", installHost.Settings.Name);
            Assert.AreEqual("42", installHost.Settings.InstanceName);
            Assert.AreEqual("Joe$42", installHost.Settings.ServiceName);
        }

        [TestMethod]
        public void Should_create_an_install_host_with_service_name_and_instance_name_no_quotes()
        {
            IHost host = HostFactory.New(x =>
            {
                x.Service<MyService>();
                x.ApplyCommandLine("install -servicename Joe -instance 42");
            });

            Assert.IsInstanceOfType(host, typeof(InstallHost));
            var installHost = (InstallHost)host;
            Assert.AreEqual("Joe", installHost.Settings.Name);
            Assert.AreEqual("42", installHost.Settings.InstanceName);
            Assert.AreEqual("Joe$42", installHost.Settings.ServiceName);
        }

        [TestMethod]
        public void Should_create_and_install_host_with_service_name_containing_space()
        {
            IHost host = HostFactory.New(x =>
            {
                x.Service<MyService>();
                x.ApplyCommandLine("install -servicename \"Joe's Service\" -instance \"42\"");
            });

            Assert.IsInstanceOfType(host, typeof(InstallHost));
            var installHost = (InstallHost)host;
            Assert.AreEqual("Joe's Service", installHost.Settings.Name);
            Assert.AreEqual("42", installHost.Settings.InstanceName);
            Assert.AreEqual("Joe's Service$42", installHost.Settings.ServiceName);
        }

        [TestMethod]
        public void Should_create_an_install_host_to_start_automatically()
        {
            IHost host = HostFactory.New(x =>
            {
                x.Service<MyService>();
                x.ApplyCommandLine("install --autostart");
            });

            Assert.IsInstanceOfType(host, typeof(InstallHost));
            var installHost = (InstallHost)host;
            Assert.AreEqual(HostStartMode.Automatic, installHost.InstallSettings.StartMode);
        }

        [TestMethod]
        public void Should_create_an_install_host_to_start_manually()
        {
            IHost host = HostFactory.New(x =>
            {
                x.Service<MyService>();
                x.ApplyCommandLine("install --manual");
            });

            Assert.IsInstanceOfType(host, typeof(InstallHost));
            var installHost = (InstallHost)host;
            Assert.AreEqual(HostStartMode.Manual, installHost.InstallSettings.StartMode);
        }

        [TestMethod]
        public void Should_create_an_install_host_to_start_manually_without_being_case_sensitive()
        {
            IHost host = HostFactory.New(x =>
            {
                x.Service<MyService>();
                x.ApplyCommandLine("InstAll --ManuAl");
            });

            Assert.IsInstanceOfType(host, typeof(InstallHost));
            var installHost = (InstallHost)host;
            Assert.AreEqual(HostStartMode.Manual, installHost.InstallSettings.StartMode);
        }

        [TestMethod]
        public void Should_create_an_install_host_to_set_disabled()
        {
            IHost host = HostFactory.New(x =>
            {
                x.Service<MyService>();
                x.ApplyCommandLine("install --disabled");
            });

            Assert.IsInstanceOfType(host, typeof(InstallHost));
            var installHost = (InstallHost)host;
            Assert.AreEqual(HostStartMode.Disabled, installHost.InstallSettings.StartMode);
        }

        [TestMethod]
        public void Should_create_an_install_host_to_start_delayed()
        {
            IHost host = HostFactory.New(x =>
            {
                x.Service<MyService>();
                x.ApplyCommandLine("install --delayed");
            });

            Assert.IsInstanceOfType(host, typeof(InstallHost));
            var installHost = (InstallHost)host;
            Assert.AreEqual(HostStartMode.AutomaticDelayed, installHost.InstallSettings.StartMode);
        }

        [TestMethod]
        public void Should_create_an_uninstall_host()
        {
            IHost host = HostFactory.New(x =>
            {
                x.Service<MyService>();
                x.ApplyCommandLine("uninstall");
            });

            Assert.IsInstanceOfType(host, typeof(UninstallHost));
        }

        [TestMethod]
        public void Should_create_a_start_host()
        {
            IHost host = HostFactory.New(x =>
            {
                x.Service<MyService>();
                x.ApplyCommandLine("start");
            });

            Assert.IsInstanceOfType(host, typeof(StartHost));
        }

        [TestMethod]
        public void Should_create_a_stop_host()
        {
            IHost host = HostFactory.New(x =>
            {
                x.Service<MyService>();
                x.ApplyCommandLine("stop");
            });

            Assert.IsInstanceOfType(host, typeof(StopHost));
        }

        [TestMethod]
        public void Extensible_the_command_line_should_be_yes()
        {
            bool isSuperfly = false;

            IHost host = HostFactory.New(x =>
            {
                x.Service<MyService>();

                //x.AddCommandLineSwitch("superfly", v => isSuperfly = v);

                x.AddCommandLineSwitch("superfly", (value) =>
                {
                    isSuperfly = value;
                });

                x.ApplyCommandLine("--superfly");
            });

            Assert.IsTrue(isSuperfly);
        }

        [TestMethod]
        public void Need_to_handle_crazy_special_characters_in_arguments()
        {
            string password = null;

            IHost host = HostFactory.New(x =>
            {
                x.Service<MyService>();

                x.AddCommandLineDefinition("password", v => password = v);

                x.ApplyCommandLine("-password:abc123!@#=$%^&*()-+");
            });

            Assert.AreEqual("abc123!@#=$%^&*()-+", password);
        }

        [TestMethod]
        public void Need_to_handle_crazy_special_characters_in_argument()
        {
            string password = null;

            IHost host = HostFactory.New(x =>
            {
                x.Service<MyService>();

                x.AddCommandLineDefinition("password", v => password = v);

                x.ApplyCommandLine("-password \"abc123=:,.<>/?;!@#$%^&*()-+\"");
            });

            Assert.AreEqual("abc123=:,.<>/?;!@#$%^&*()-+", password);
        }

        [TestMethod]
        public void Need_to_handle_crazy_special_characters_in_argument_no_quotes()
        {
            string password = null;

            IHost host = HostFactory.New(x =>
            {
                x.Service<MyService>();

                x.AddCommandLineDefinition("password", v => password = v);

                x.ApplyCommandLine("-password abc123=:,.<>/?;!@#$%^&*()-+");
            });

            Assert.AreEqual("abc123=:,.<>/?;!@#$%^&*()-+", password);
        }

        [TestMethod]
        public void Extensible_the_command_line_should_be_yet_again()
        {
            string volumeLevel = null;

            IHost host = HostFactory.New(x =>
            {
                x.Service<MyService>();

                x.AddCommandLineDefinition("volumeLevel", v => volumeLevel = v);

                x.ApplyCommandLine("-volumeLevel:11");
            });

            Assert.AreEqual("11", volumeLevel);
        }




        class MyService : ServiceControl
        {
            public MyService()
            {
            }

            public bool Start(IHostControl hostControl)
            {
                throw new NotImplementedException();
            }

            public bool Stop(IHostControl hostControl)
            {
                throw new NotImplementedException();
            }

            public bool Pause(IHostControl hostControl)
            {
                throw new NotImplementedException();
            }

            public bool Continue(IHostControl hostControl)
            {
                throw new NotImplementedException();
            }
        }
    }
}
