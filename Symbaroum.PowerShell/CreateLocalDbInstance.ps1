#
# CreateLocalDbInstance.ps1
#
SqlLocalDb stop SymbLocalDb
#Remove-Item 'C:\users\krister\Symbaroum.mdf'
#Remove-Item 'C:\users\krister\Symbaroum_log.ldf'
SqlLocalDb create SymbLocalDb