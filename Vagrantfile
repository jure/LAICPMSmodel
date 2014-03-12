# -*- mode: ruby -*-
# vi: set ft=ruby :

$script = <<SCRIPT
sudo apt-get update --yes
sudo apt-get install python-software-properties --yes
sudo add-apt-repository ppa:ubuntu-wine/ppa --yes
sudo apt-get update --yes
sudo apt-get install wine1.7 --yes --force-yes
SCRIPT

VAGRANTFILE_API_VERSION = "2"

Vagrant.configure(VAGRANTFILE_API_VERSION) do |config|
  config.vm.provision "shell", inline: $script
  config.vm.box = "hashicorp/precise64"
  config.ssh.forward_x11
end
