CREATE NONCLUSTERED INDEX [IX_Downline] ON [dbo].[Downline] ([CategoryId], [DownlineId])
GO
IF @@ERROR<>0 AND @@TRANCOUNT>0 ROLLBACK TRANSACTION
GO
IF @@TRANCOUNT=0 BEGIN INSERT INTO #tmpErrors (Error) SELECT 1 BEGIN TRANSACTION END
GO
