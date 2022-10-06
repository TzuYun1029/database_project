using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using MySql.Data.MySqlClient;
using System.Data;


namespace dbms_final_project {

    public partial class Form1 : Form {


        public Form1() {
            InitializeComponent();
            comboBox1.Items.Add("MYSQL");
            comboBox1.Items.Add("SELECT-FROM-WHERE 專輯中的歌曲與年份");
            comboBox1.Items.Add("DELETE 刪除使用者喜歡的歌");
            comboBox1.Items.Add("INSERT 新增使用者喜歡的歌");
            comboBox1.Items.Add("UPDATE 使用者更改名字");
            comboBox1.Items.Add("IN 播放清單共編者名字和ID");
            comboBox1.Items.Add("NOT IN 別錯過這些台語歌");
            comboBox1.Items.Add("EXISTS 相關的播放清單");
            comboBox1.Items.Add("NOT EXISTS 沒有喜歡日文歌的使用者");
            comboBox1.Items.Add("COUNT 中文歌排行榜(依播放次數)");
            comboBox1.Items.Add("SUM 歌手專輯總長度(秒)");
            comboBox1.Items.Add("MAX 歌手最新發行");
            comboBox1.Items.Add("MIN 歌手最舊專輯");
            comboBox1.Items.Add("AVG 使用者六月平均每日播放歌曲數");
            comboBox1.Items.Add("HAVING 使用者年度喜愛歌手(聽歌5次以上)");
            msg.Text = "";
            sql_label.Text = "";
            sql_label.ReadOnly = true;
            sql_label.BorderStyle = 0;
            sql_label.BackColor = this.BackColor;
            sql_label.TabStop = false;
            sql_label.Multiline = true;
        }
        public bool read = false;
        

        private void button1_Click(object sender, EventArgs e) {

            set_sql();
            MySqlConnection conn = new MySqlConnection();
            string connString = "server=127.0.0.1;port=3306;user id=root;password=;database=dbms_final;charset=utf8;";

            conn.ConnectionString = connString;
            if (conn.State != ConnectionState.Open) {
                try {
                    conn.Open();
                    msg.Text = "DB connected";
                    msg.ForeColor = Color.FromArgb(0, 150, 0);
                } catch {
                    msg.Text = "Fail to connect to DB";
                    msg.ForeColor = Color.FromArgb(200, 0, 0);
                    return;
                }

                string sql = sql_label.Text;
           
                
                MySqlCommand cmd = new MySqlCommand(sql, conn);
      
                try {
                    int index = cmd.ExecuteNonQuery();
                    msg.Text = "SQL successfully sent";
                    msg.ForeColor = Color.FromArgb(0, 150, 0);
                } catch {
                    msg.Text = "SQL wrong";
                    msg.ForeColor = Color.FromArgb(200, 0, 0);
                    conn.Close();
                    return;
                }

              
                if (read) {
                    try {
                        MySqlDataAdapter ad = new MySqlDataAdapter();
                        ad.SelectCommand = cmd;
                        DataTable dt = new DataTable();
                        ad.Fill(dt);
                        dataGridView1.DataSource = dt;
                        msg.Text = "";
                    } catch {
                        msg.Text = "fail to display";
                        msg.ForeColor = Color.FromArgb(50, 50, 50);
                    }

                } else {
                    dataGridView1.DataSource = null;
                }
                    conn.Close();
                }
            }
        
        private void apply_button_Click(object sender, EventArgs e) {

            DateTime localDate = DateTime.Now;
            string time = localDate.ToString("yyyy/MM/dd HH:mm:ss");

            if (comboBox1.Text == "time") {
                sql_cmd.Text = time;
            }

            // 專輯中的歌曲與年份 ok
            if (comboBox1.Text == "SELECT-FROM-WHERE 專輯中的歌曲與年份") {
                sql_cmd.Text = "SELECT songName,albumYear \r\n" +
                                "FROM song,album \r\n" +
                                "WHERE (songAlbumId = albumId AND albumId='0');";
                read = true;
            } 
            // 刪除喜歡的歌 ok
            else if (comboBox1.Text == "DELETE 刪除使用者喜歡的歌") {
                sql_cmd.Text = "DELETE FROM like_song \r\n" +
                                "WHERE userId = '0' AND songId = '0';";
                read = false;
            }
            // 新增喜歡的歌 ok
            else if (comboBox1.Text == "INSERT 新增使用者喜歡的歌") {
                sql_cmd.Text = "INSERT INTO like_song (userId, songId, likeTime) \r\n" +
                                "VALUES('0', '0', '"+time+"');";
                read = false;
            } 
            // 使用者更改名字 ok
            else if (comboBox1.Text == "UPDATE 使用者更改名字") {
                sql_cmd.Text = "UPDATE user \r\n" +
                                "SET userName = 'Banana' WHERE userId = '9';";
                read = false;
            } 
            // 播放清單共編者名字和ID ok
            else if (comboBox1.Text == "IN 播放清單共編者名字和ID") {
                sql_cmd.Text = "SELECT userName,userId \r\n" +
                                "FROM user WHERE userId IN \r\n" +
                                "(SELECT userId FROM add_to_playlist WHERE playlistId = '0');";
                read = true;
            } 
            // 別錯過這些中文歌 ok
            else if (comboBox1.Text == "NOT IN 別錯過這些台語歌") {
                sql_cmd.Text = "SELECT songName,artistName \r\n" +
                                "FROM song S, artist A, make_song MS \r\n" +
                                "WHERE(A.artistId = MS.artistId AND S.songId = MS.songId AND S.songLanguage = '台語') \r\n" +
                                "AND S.songId NOT IN(SELECT songId FROM listen_to_song WHERE userId = '9'); ";
                read = true;
            } 
            // 相關的播放清單 ok
            else if (comboBox1.Text == "EXISTS 相關的播放清單") {
                sql_cmd.Text = "SELECT playlistName \r\n" +
                                "FROM playlist P \r\n" +
                                "WHERE EXISTS \r\n" +
                                "(SELECT * FROM add_to_playlist A, song S \r\n" +
                                "WHERE P.playlistId = A.playlistId AND P.playlistPrivacy = '公開' \r\n" +
                                "AND A.songId = S.songId AND S.songName = '愛情的模樣'); ";
                read = true;
            }
            // 沒有喜歡日文歌的使用者
            else if (comboBox1.Text == "NOT EXISTS 沒有喜歡日文歌的使用者") {
                sql_cmd.Text = "SELECT U.userId, U.userName \r\n" +
                                "FROM user U \r\n" +
                                "WHERE NOT EXISTS \r\n" +
                                "(SELECT* \r\n" +
                                "FROM like_song L, song S \r\n" +
                                "WHERE L.songId = S.songId AND L.userId = U.userId AND S.songLanguage = '日文'); ";
                read = true;
            }
            // 中文歌排行榜(依播放次數) ok
            else if (comboBox1.Text == "COUNT 中文歌排行榜(依播放次數)") {
                sql_cmd.Text = "SELECT songName, COUNT(*) \r\n" +
                                "FROM listen_to_song L, song S \r\n" +
                                "WHERE L.songId = S.songId AND S.songLanguage = '中文' GROUP BY L.songId ORDER BY COUNT(*) DESC; ";
                read = true;
            }
            // 歌手專輯總長度(秒) ok
            else if (comboBox1.Text == "SUM 歌手專輯總長度(秒)") {
                sql_cmd.Text = "SELECT artistName, albumName, albumType, SUM(songTime) \r\n" +
                                "FROM album A, song S, artist AR \r\n" +
                                "WHERE A.albumId = S.songAlbumId AND A.albumArtistId = AR.artistId AND A.albumArtistId = '1' GROUP BY S.songAlbumId; ";
                read = true;
            }
            // 歌手最新發行 ok
            else if (comboBox1.Text == "MAX 歌手最新發行") {
                sql_cmd.Text = "SELECT albumYear, albumName, artistName, albumType \r\n" +
                                "FROM album, artist, (SELECT MAX(albumYear) m FROM album WHERE albumArtistId = '0' GROUP BY albumArtistId)M \r\n" +
                                "WHERE M.m = albumYear AND albumArtistId = '0' AND albumArtistId = artistId; ";
                read = true;
            }
            // 歌手最舊專輯 ok
            else if (comboBox1.Text == "MIN 歌手最舊專輯") {
                sql_cmd.Text = "SELECT albumYear, albumName, artistName, albumType \r\n" +
                                "FROM album, artist, (SELECT MIN(albumYear) m FROM album WHERE albumArtistId = '0' GROUP BY albumArtistId)M \r\n" +
                                "WHERE M.m = albumYear AND albumArtistId = '0' AND albumArtistId = artistId; ";
                read = true;
            }
            // 使用者六月平均每日播放歌曲數 ok
            else if (comboBox1.Text == "AVG 使用者六月平均每日播放歌曲數") {
                sql_cmd.Text = "SELECT  U.userId, U.userName, song_per_day \r\n" +
                    "FROM \r\n" +
                    "(SELECT userId, AVG(count) song_per_day \r\n" +
                    "FROM " +
                    "(SELECT userId, SUBSTR(listenTime, 9, 2) date, COUNT(*) count \r\n" +
                    "FROM listen_to_song \r\n" +
                    "GROUP BY SUBSTR(listenTime, 9, 2), userId) P \r\n" +
                    "GROUP BY userId)P, user U \r\n" +
                    "WHERE U.userId = P.userId; ";
                read = true;
            }
            // 使用者年度喜愛歌手(聽歌5次以上) ok
            else if (comboBox1.Text == "HAVING 使用者年度喜愛歌手(聽歌5次以上)") {
                sql_cmd.Text = "SELECT U.userName, A.artistName, COUNT(*) listen_times, SUBSTR(L.listenTime, 1,4) year \r\n" +
                                "FROM user U, artist A, make_song MS, listen_to_song L \r\n" +
                                "WHERE U.userId = L.userId AND L.songId = MS.songId AND MS.artistId = A.artistId \r\n" +
                                "GROUP BY U.userId, A.artistId, SUBSTR(L.listenTime, 1, 4) \r\n" +
                                "HAVING listen_times >= 5; ";
                read = true;
            } 
            else {
                msg.Text = "no instruction selected";
                msg.ForeColor = Color.FromArgb(100, 100, 100);
            }
        }



        private void set_sql() {

            DateTime localDate = DateTime.Now;
            string time = localDate.ToString("yyyy/MM/dd HH:mm:ss");

            if (comboBox1.Text == "time") {
                sql_label.Text = time;
            }

            // 專輯中的歌曲與年份 ok
            if (comboBox1.Text == "SELECT-FROM-WHERE 專輯中的歌曲與年份") {
                sql_label.Text = "SELECT songName,albumYear \r\n" +
                                "FROM song,album \r\n" +
                                "WHERE (songAlbumId = albumId AND albumId='0');";
                read = true;
            }
            // 刪除喜歡的歌 ok
            else if (comboBox1.Text == "DELETE 刪除使用者喜歡的歌") {
                sql_label.Text = "DELETE FROM like_song \r\n" +
                                "WHERE userId = '0' AND songId = '0';";
                read = false;
            }
            // 新增喜歡的歌 ok
            else if (comboBox1.Text == "INSERT 新增使用者喜歡的歌") {
                sql_label.Text = "INSERT INTO like_song (userId, songId, likeTime) \r\n" +
                                "VALUES('0', '0', '" + time + "');";
                read = false;
            }
            // 使用者更改名字 ok
            else if (comboBox1.Text == "UPDATE 使用者更改名字") {
                sql_label.Text = "UPDATE user \r\n" +
                                "SET userName = 'Banana' WHERE userId = '9';";
                read = false;
            }
            // 播放清單共編者名字和ID ok
            else if (comboBox1.Text == "IN 播放清單共編者名字和ID") {
                sql_label.Text = "SELECT userName,userId \r\n" +
                                "FROM user WHERE userId IN \r\n" +
                                "(SELECT userId FROM add_to_playlist WHERE playlistId = '0');";
                read = true;
            }
            // 別錯過這些中文歌 ok
            else if (comboBox1.Text == "NOT IN 別錯過這些台語歌") {
                sql_label.Text = "SELECT songName,artistName \r\n" +
                                "FROM song S, artist A, make_song MS \r\n" +
                                "WHERE(A.artistId = MS.artistId AND S.songId = MS.songId AND S.songLanguage = '台語') \r\n" +
                                "AND S.songId NOT IN(SELECT songId FROM listen_to_song WHERE userId = '9'); ";
                read = true;
            }
            // 相關的播放清單 ok
            else if (comboBox1.Text == "EXISTS 相關的播放清單") {
                sql_label.Text = "SELECT playlistName \r\n" +
                                "FROM playlist P \r\n" +
                                "WHERE EXISTS \r\n" +
                                "(SELECT * FROM add_to_playlist A, song S \r\n" +
                                "WHERE P.playlistId = A.playlistId AND P.playlistPrivacy = '公開' \r\n" +
                                "AND A.songId = S.songId AND S.songName = '愛情的模樣'); ";
                read = true;
            }
            // 沒有喜歡日文歌的使用者
            else if (comboBox1.Text == "NOT EXISTS 沒有喜歡日文歌的使用者") {
                sql_label.Text = "SELECT U.userId, U.userName \r\n" +
                                "FROM user U \r\n" +
                                "WHERE NOT EXISTS \r\n" +
                                "(SELECT* \r\n" +
                                "FROM like_song L, song S \r\n" +
                                "WHERE L.songId = S.songId AND L.userId = U.userId AND S.songLanguage = '日文'); ";
                read = true;
            }
            // 中文歌排行榜(依播放次數) ok
            else if (comboBox1.Text == "COUNT 中文歌排行榜(依播放次數)") {
                sql_label.Text = "SELECT songName, COUNT(*) \r\n" +
                                "FROM listen_to_song L, song S \r\n" +
                                "WHERE L.songId = S.songId AND S.songLanguage = '中文' GROUP BY L.songId ORDER BY COUNT(*) DESC; ";
                read = true;
            }
            // 歌手專輯總長度(秒) ok
            else if (comboBox1.Text == "SUM 歌手專輯總長度(秒)") {
                sql_label.Text = "SELECT artistName, albumName, albumType, SUM(songTime) \r\n" +
                                "FROM album A, song S, artist AR \r\n" +
                                "WHERE A.albumId = S.songAlbumId AND A.albumArtistId = AR.artistId AND A.albumArtistId = '1' GROUP BY S.songAlbumId; ";
                read = true;
            }
            // 歌手最新發行 ok
            else if (comboBox1.Text == "MAX 歌手最新發行") {
                sql_label.Text = "SELECT albumYear, albumName, artistName, albumType \r\n" +
                                "FROM album, artist, (SELECT MAX(albumYear) m FROM album WHERE albumArtistId = '0' GROUP BY albumArtistId)M \r\n" +
                                "WHERE M.m = albumYear AND albumArtistId = '0' AND albumArtistId = artistId; ";
                read = true;
            }
            // 歌手最舊專輯 ok
            else if (comboBox1.Text == "MIN 歌手最舊專輯") {
                sql_label.Text = "SELECT albumYear, albumName, artistName, albumType \r\n" +
                                "FROM album, artist, (SELECT MIN(albumYear) m FROM album WHERE albumArtistId = '0' GROUP BY albumArtistId)M \r\n" +
                                "WHERE M.m = albumYear AND albumArtistId = '0' AND albumArtistId = artistId; ";
                read = true;
            }
            // 使用者六月平均每日播放歌曲數 ok
            else if (comboBox1.Text == "AVG 使用者六月平均每日播放歌曲數") {
                sql_label.Text = "SELECT  U.userId, U.userName, song_per_day \r\n" +
                    "FROM \r\n" +
                    "(SELECT userId, AVG(count) song_per_day \r\n" +
                    "FROM " +
                    "(SELECT userId, SUBSTR(listenTime, 9, 2) date, COUNT(*) count \r\n" +
                    "FROM listen_to_song \r\n" +
                    "GROUP BY SUBSTR(listenTime, 9, 2), userId) P \r\n" +
                    "GROUP BY userId)P, user U \r\n" +
                    "WHERE U.userId = P.userId; ";
                read = true;
            }
            // 使用者年度喜愛歌手(聽歌5次以上) ok
            else if (comboBox1.Text == "HAVING 使用者年度喜愛歌手(聽歌5次以上)") {
                sql_label.Text = "SELECT U.userName, A.artistName, COUNT(*) listen_times, SUBSTR(L.listenTime, 1,4) year \r\n" +
                                "FROM user U, artist A, make_song MS, listen_to_song L \r\n" +
                                "WHERE U.userId = L.userId AND L.songId = MS.songId AND MS.artistId = A.artistId \r\n" +
                                "GROUP BY U.userId, A.artistId, SUBSTR(L.listenTime, 1, 4) \r\n" +
                                "HAVING listen_times >= 5; ";
                read = true;
            } else if (comboBox1.Text == "MYSQL") {
                sql_label.Text = sql_cmd.Text;
                // msg.Text = "no instruction selected";
                // msg.ForeColor = Color.FromArgb(100, 100, 100);
                return;

            }
            sql_cmd.Text = "";
        }

        private void msg_Click(object sender, EventArgs e) {

        }
    }
}
