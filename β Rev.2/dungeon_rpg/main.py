import pygame
from pygame.locals import *
import tkinter
from tkinter import messagebox
import sys
import os
import random

width, height = (640, 480)
SCREEN_SIZE = (width + 200, height)
game_status = "DUNGEON"
"""
game_statusはゲーム状態を表す変数。
DUNGEON -> ダンジョンのマップを動ける状態
BATTLE -> バトル
GAME_OVER -> ゲームオーバー
"""

class Player:
    def __init__(self):
        self.direction = 4 #Playerの向き
        self.x = 2
        self.y = 16
        self.dungeon = 0
        self.floor = 0


# コンパニオンクラス
# コンパニオンのステータスは全員分をリストで管理
class Companion(Player):
    def __init__(self):
        super(Companion, self).__init__()
        # コンパニオンリスト -> ["名前", "職業", Lv , HP, MP, ATK, DEF, INT, MDF]
        # パラメータの割り振りは後で
        self.companion_list = [["ショコラ", "Warrior", 1, 0, 0, 0, 0, 0, 0],
                               ["もなか", "Mage", 1, 0, 0, 0, 0, 0, 0],
                               ["メロリン", "Priest", 1, 0, 0, 0, 0, 0, 0],
                               ["いちご", "Guard", 1, 0, 0, 0, 0, 0, 0]]
        # ジョブごとのパラメータの割り振り ([i][2]はLv.です)
        # 値は仮のものです (後で修正します)
        for i in range(4):
            if self.companion_list[i][1] == "Warrior":
                self.companion_list[i][3] = 100 + self.companion_list[i][2] * 5  # HP
                self.companion_list[i][4] = 10 + self.companion_list[i][2] * 1  # MP
                self.companion_list[i][5] = 40 + self.companion_list[i][2] * 5  # ATK
                self.companion_list[i][6] = 35 + self.companion_list[i][2] * 3  # DEF
                self.companion_list[i][7] = 13 + self.companion_list[i][2] * 1  # INT
                self.companion_list[i][8] = 14 + self.companion_list[i][2] * 1  # MDF
            if self.companion_list[i][1] == "Mage":
                self.companion_list[i][3] = 54 + self.companion_list[i][2] * 2  # HP
                self.companion_list[i][4] = 100 + self.companion_list[i][2] * 5  # MP
                self.companion_list[i][5] = 4 + self.companion_list[i][2] * 1  # ATK
                self.companion_list[i][6] = 12 + self.companion_list[i][2] * 2  # DEF
                self.companion_list[i][7] = 38 + self.companion_list[i][2] * 4  # INT
                self.companion_list[i][8] = 40 + self.companion_list[i][2] * 2  # MDF
            if self.companion_list[i][1] == "Priest":
                self.companion_list[i][3] = 55 + self.companion_list[i][2] * 3  # HP
                self.companion_list[i][4] = 66 + self.companion_list[i][2] * 3  # MP
                self.companion_list[i][5] = 23 + self.companion_list[i][2] * 2  # ATK
                self.companion_list[i][6] = 34 + self.companion_list[i][2] * 3  # DEF
                self.companion_list[i][7] = 33 + self.companion_list[i][2] * 2  # INT
                self.companion_list[i][8] = 50 + self.companion_list[i][2] * 4  # MDF
            if self.companion_list[i][1] == "Guard":
                self.companion_list[i][3] = 130 + self.companion_list[i][2] * 5  # HP
                self.companion_list[i][4] = 10 + self.companion_list[i][2] * 1  # MP
                self.companion_list[i][5] = 60 + self.companion_list[i][2] * 2  # ATK
                self.companion_list[i][6] = 50 + self.companion_list[i][2] * 5  # DEF
                self.companion_list[i][7] = 8 + self.companion_list[i][2] * 1  # INT
                self.companion_list[i][8] = 32 + self.companion_list[i][2] * 3  # MDF
        # HP, MPの定義(初期値は最大値を使用)
        self.companion_hp = [self.companion_list[0][3],
                             self.companion_list[1][3],
                             self.companion_list[2][3],
                             self.companion_list[3][3]]
        self.companion_mp = [self.companion_list[0][4],
                             self.companion_list[1][4],
                             self.companion_list[2][4],
                             self.companion_list[3][4]]


# 敵もコンパニオン同様リストで管理
class Enemy:
    def __init__(self):
        # エネミーリスト -> ["名前", EXP, Lv , HP, MP, ATK, DEF, INT, MDF, filename]
        self.enemy_list = [["スライミー", 10, 1, 80, 0, 10, 10, 10, 10, "data/image/slimy.png"]]

class Map:
    def __init__(self, player, graphic, fonts, battle,  filename):
        self.width = -1
        self.height = -1
        self.map_gridsize = 6
        self.player = player
        self.graphic = graphic
        self.filename = filename
        self.fonts = fonts
        self.battle = battle
        """
        self.map属性はマップの設計図です
        0 -> 普通の通路(未探索)
        1 -> 壁
        2 -> 出入口、階段
        3 -> 探索済み
        """
        self.map = []
        self.map_masked = []
        self.load_map()

    def load_map(self):
        file = os.path.join("data/mapdata", self.filename)  # マップデータのpath代入
        mapdata = open(file)  # mapファイル読み込み
        lines = mapdata.readlines() # 行の読み込み
        width_str, height_str = lines[0].split()
        self.width, self.height = int(width_str), int(height_str)  # マップサイズの縦横の大きさ取得
        crntdngn_str, crntflr_str = lines[1].split()
        self.player.dungeon, self.player.floor = int(crntdngn_str),  int(crntflr_str)  # 現在地(ダンジョン、階)取得
        for line in lines[2:]:  # 3行目からのマップデータ読み込み
            line = line.rstrip() # 改行の読み飛ばし
            self.map.append([int(x) for x in list(line)])
        mapdata.close()

    def trigger(self):
        self.draw_display()
        self.draw_minimap()
        self.move()

    def draw_minimap(self):
        # ミニマップ全体像の描画
        # 未探索部分を隠すための描画
        for x in range(self.width):
            for y in range(self.height):
                if self.map[y][x] == 0:
                    pygame.draw.rect(screen, (0, 0, 64), (
                    x * self.map_gridsize, y * self.map_gridsize, self.map_gridsize, self.map_gridsize))
                elif self.map[y][x] == 1:
                    pygame.draw.rect(screen, (0, 0, 64), (
                    x * self.map_gridsize, y * self.map_gridsize, self.map_gridsize, self.map_gridsize))
                elif self.map[y][x] == 2:
                    pygame.draw.rect(screen, (0, 0, 64), (
                    x * self.map_gridsize, y * self.map_gridsize, self.map_gridsize, self.map_gridsize))
                elif self.map[y][x] == 3:
                    pygame.draw.rect(screen, (191, 191, 255), (
                    x * self.map_gridsize, y * self.map_gridsize, self.map_gridsize, self.map_gridsize))
                elif self.map[y][x] == 4:
                    pygame.draw.rect(screen, (255, 255, 0), (
                    x * self.map_gridsize, y * self.map_gridsize, self.map_gridsize, self.map_gridsize))


        # プレイヤー現在地の提示
        pygame.draw.rect(screen, (255, 0, 0), (self.player.x * self.map_gridsize, self.player.y * self.map_gridsize, self.map_gridsize, self.map_gridsize))

    def draw_display(self):
        self.graphic.draw_floor()
        # プレイヤーの向き別壁の描画
        # 北向き
        if self.player.direction == 4:
            # 奥正面
            if self.map[self.player.y - 2][self.player.x] == 1:
                self.graphic.draw_farside_center()
            # 奥左
            if self.map[self.player.y - 2][self.player.x - 1] == 1:
                self.graphic.draw_farside_left()
            # 奥右
            if self.map[self.player.y - 2][self.player.x + 1] == 1:
                self.graphic.draw_farside_right()
            # 中正面
            if self.map[self.player.y - 1][self.player.x] == 1:
                self.graphic.draw_middle_center()
            # 中左
            if self.map[self.player.y - 1][self.player.x - 1] == 1:
                self.graphic.draw_middle_left()
            # 中右
            if self.map[self.player.y - 1][self.player.x + 1] == 1:
                self.graphic.draw_middle_right()
            # 手前左
            if self.map[self.player.y][self.player.x - 1] == 1:
                self.graphic.draw_nearside_left()
            # 手前右
            if self.map[self.player.y][self.player.x + 1] == 1:
                self.graphic.draw_nearside_right()
        # 東向き
        if self.player.direction == 3:
            # 奥正面
            if self.map[self.player.y][self.player.x + 2] == 1:
                self.graphic.draw_farside_center()
            # 奥左
            if self.map[self.player.y - 1][self.player.x + 2] == 1:
                self.graphic.draw_farside_left()
            # 奥右
            if self.map[self.player.y + 1][self.player.x + 2] == 1:
                self.graphic.draw_farside_right()
            # 中正面
            if self.map[self.player.y][self.player.x + 1] == 1:
                self.graphic.draw_middle_center()
            # 中左
            if self.map[self.player.y - 1][self.player.x + 1] == 1:
                self.graphic.draw_middle_left()
            # 中右
            if self.map[self.player.y + 1][self.player.x + 1] == 1:
                self.graphic.draw_middle_right()
            # 手前左
            if self.map[self.player.y - 1][self.player.x] == 1:
                self.graphic.draw_nearside_left()
            # 手前右
            if self.map[self.player.y + 1][self.player.x] == 1:
                self.graphic.draw_nearside_right()
        # 南向き
        if self.player.direction == 2:
            # 奥正面
            if self.map[self.player.y + 2][self.player.x] == 1:
                self.graphic.draw_farside_center()
            # 奥左
            if self.map[self.player.y + 2][self.player.x + 1] == 1:
                self.graphic.draw_farside_left()
            # 奥右
            if self.map[self.player.y + 2][self.player.x - 1] == 1:
                self.graphic.draw_farside_right()
            # 中正面
            if self.map[self.player.y + 1][self.player.x] == 1:
                self.graphic.draw_middle_center()
            # 中左
            if self.map[self.player.y + 1][self.player.x + 1] == 1:
                self.graphic.draw_middle_left()
            # 中右
            if self.map[self.player.y + 1][self.player.x - 1] == 1:
                self.graphic.draw_middle_right()
            # 手前左
            if self.map[self.player.y][self.player.x + 1] == 1:
                self.graphic.draw_nearside_left()
            # 手前右
            if self.map[self.player.y][self.player.x - 1] == 1:
                self.graphic.draw_nearside_right()
        # 西向き
        if self.player.direction == 1:
            # 奥正面
            if self.map[self.player.y][self.player.x - 2] == 1:
                self.graphic.draw_farside_center()
            # 奥左
            if self.map[self.player.y + 1][self.player.x - 2] == 1:
                self.graphic.draw_farside_left()
            # 奥右
            if self.map[self.player.y - 1][self.player.x - 2] == 1:
                self.graphic.draw_farside_right()
            # 中正面
            if self.map[self.player.y][self.player.x - 1] == 1:
                self.graphic.draw_middle_center()
            # 中左
            if self.map[self.player.y + 1][self.player.x - 1] == 1:
                self.graphic.draw_middle_left()
            # 中右
            if self.map[self.player.y - 1][self.player.x - 1] == 1:
                self.graphic.draw_middle_right()
            # 手前左
            if self.map[self.player.y + 1][self.player.x] == 1:
                self.graphic.draw_nearside_left()
            # 手前右
            if self.map[self.player.y - 1][self.player.x] == 1:
                self.graphic.draw_nearside_right()

    def move(self):
        global game_status
        if game_status == "DUNGEON":  # ダンジョン探索中のみ移動キーを開放
            global pressed_key
            if pressed_key:
                pygame.time.wait(150)  # そのままだと暴走してどんどん進むので待機時間150ms
            if pressed_key[K_a]:
                self.player.direction += 1
                if self.player.direction == 5:
                    self.player.direction = 1
            elif pressed_key[K_d]:
                self.player.direction -= 1
                if self.player.direction == 0:
                    self.player.direction = 4
            if self.player.direction == 4: # 北を向いているとき
                if pressed_key[K_w]:
                    if self.map[self.player.y - 1][self.player.x] != 1:
                        self.player.y -= 1
                elif pressed_key[K_s]:
                    if self.map[self.player.y + 1][self.player.x] != 1:
                        self.player.y += 1
            elif self.player.direction == 3: # 東を向いているとき
                if pressed_key[K_w]:
                    if self.map[self.player.y][self.player.x + 1] != 1:
                        self.player.x += 1
                elif pressed_key[K_s]:
                    if self.map[self.player.y][self.player.x - 1] != 1:
                        self.player.x -= 1
            elif self.player.direction == 2: # 南を向いているとき
                if pressed_key[K_w]:
                    if self.map[self.player.y + 1][self.player.x] != 1:
                        self.player.y += 1
                elif pressed_key[K_s]:
                    if self.map[self.player.y - 1][self.player.x] != 1:
                        self.player.y -= 1
            elif self.player.direction == 1: # 西を向いているとき
                if pressed_key[K_w]:
                    if self.map[self.player.y][self.player.x - 1] != 1:
                        self.player.x -= 1
                elif pressed_key[K_s]:
                    if self.map[self.player.y][self.player.x + 1] != 1:
                        self.player.x += 1
            if pressed_key[K_w] or pressed_key[K_s] :
                if self.map[self.player.y][self.player.x] == 0: #未探索ブロックを踏んだら
                    self.map[self.player.y][self.player.x] = 3  # 探索済みのブロックへ
                elif self.map[self.player.y][self.player.x] == 2: # 未探索階段を踏んだら
                    self.map[self.player.y][self.player.x] = 4  # 探索済み階段へ
                battle_dice = random.randint(0, 32)  # バトル発生抽選
                if battle_dice == 0:
                    game_status = "BATTLE"
                    self.battle.battle_status = "INIT"


class Graphic:
    def __init__(self):
        pass
    def draw_floor(self):
        pygame.draw.rect(screen, (255, 255, 255), (0, 0, width, height * (10 / 16)))
        pygame.draw.polygon(screen, (255, 255, 255), [[width, height], [0, height],
                                                      [width * (6 / 16), height * (10 / 16)],
                                                      [width * (10 / 16), height * (10 / 16)]])
    def draw_farside_center(self):
        pygame.draw.rect(screen, (127, 127, 127),
                         (width * (5 / 16), height * (5 / 16), width * (6 / 16), height * (6 / 16)))
    def draw_farside_left(self):
        pygame.draw.rect(screen, (127, 127, 127), (0, height * (5 / 16), width * (5 / 16), height * (6 / 16)))
        pygame.draw.polygon(screen, (127, 127, 127), [[width * (5 / 16), height * (11 / 16)],
                                                      [width * (5 / 16), height * (5 / 16)],
                                                      [width * (6 / 16), height * (6 / 16)],
                                                      [width * (6 / 16), height * (10 / 16)]])
    def draw_farside_right(self):
        pygame.draw.rect(screen, (127, 127, 127),
                         (width * (11 / 16), height * (5 / 16), width * (5 / 16), height * (6 / 16)))
        pygame.draw.polygon(screen, (127, 127, 127), [[width * (11 / 16), height * (11 / 16)],
                                                      [width * (11 / 16), height * (5 / 16)],
                                                      [width * (10 / 16), height * (6 / 16)],
                                                      [width * (10 / 16), height * (10 / 16)]])
    def draw_middle_center(self):
        pygame.draw.rect(screen, (127, 127, 127),
                         (width * (3 / 16), height * (3 / 16), width * (10 / 16), height * (10 / 16)))
    def draw_middle_left(self):
        pygame.draw.rect(screen, (127, 127, 127),
                         (0, height * (3 / 16), width * (3 / 16), height * (10 / 16)))
        pygame.draw.polygon(screen, (127, 127, 127), [[width * (3 / 16), height * (3 / 16)],
                                                      [width * (3 / 16), height * (13 / 16)],
                                                      [width * (5 / 16), height * (11 / 16)],
                                                      [width * (5 / 16), height * (5 / 16)]])
    def draw_middle_right(self):
        pygame.draw.rect(screen, (127, 127, 127),
                         (width * (13 / 16), height * (3 / 16), width * (3 / 16), height * (10 / 16)))
        pygame.draw.polygon(screen, (127, 127, 127), [[width * (13 / 16), height * (3 / 16)],
                                                      [width * (13 / 16), height * (13 / 16)],
                                                      [width * (11 / 16), height * (11 / 16)],
                                                      [width * (11 / 16), height * (5 / 16)]])
    def draw_nearside_left(self):
        pygame.draw.polygon(screen, (127, 127, 127), [[0, 0],
                                                      [0, height],
                                                      [width * (3 / 16), height * (13 / 16)],
                                                      [width * (3 / 16), height * (3 / 16)]])
    def draw_nearside_right(self):
        pygame.draw.polygon(screen, (127, 127, 127), [[width, 0],
                                                      [width, height],
                                                      [width * (13 / 16), height * (13 / 16)],
                                                      [width * (13 / 16), height * (3 / 16)]])

class Info:
    def __init__(self, fonts, player, companions, map):
        self.fonts = fonts
        self.player = player
        self.map = map
        self.companions = companions

    def draw_info(self):
        self.draw_info_frame()
        self.draw_info_text()

    def draw_info_frame(self):
        pygame.draw.rect(screen, (0, 0, 0), (width, 0, 200, height))
        pygame.draw.rect(screen, (255, 255, 255), (width, 0, 200, height), 3)

    def draw_info_text(self):
        # 現在地
        floor_info_msg = "Floor " + str(self.player.dungeon) + " - " + str(self.player.floor)
        floor_info = self.fonts.info_font.render(floor_info_msg, True, (255, 255, 255))
        screen.blit(floor_info, (width + 100 - floor_info.get_width() / 2, 5))
        #  コンパニオンステータス
        for i in range(4):
            # 名前の表示
            comp_name = self.companions.companion_list[i][0] + " L:" + str(self.companions.companion_list[i][2])
            comp_name_font = self.fonts.info_font.render(comp_name, True, (255, 255, 255))
            screen.blit(comp_name_font, (width + 3, i * 96 + 64))
            # HPの表示
            comp_hp = "HP " + str(self.companions.companion_hp[i]) + "/" + str(self.companions.companion_list[i][3])
            comp_hp_font = self.fonts.info_font.render(comp_hp, True, (255, 255, 255))
            screen.blit(comp_hp_font, (width + 27, i * 96 + 88))
            # MPの表示
            comp_mp = "MP " + str(self.companions.companion_mp[i]) + "/" + str(self.companions.companion_list[i][4])
            comp_mp_font = self.fonts.info_font.render(comp_mp, True, (255, 255, 255))
            screen.blit(comp_mp_font, (width + 27, i * 96 + 112))

class Battle:
    def __init__(self, fonts, companions, enemy):
        """
        battle_status: バトルの状態を表す
        INIT -> 開始時の初期設定
        COMMAND -> コマンド入力
        PROCESS_PLAYER -> 味方の行動
        PROCESS_ENEMY -> 敵の行動
        WIN -> 勝利
        GAME_OVER -> 敗北
        """
        self.battle_status = "INIT"
        self.fonts = fonts
        self.companions = companions
        self.enemy = enemy
        self.cmd_selecting = 0  # コマンドカーソル位置
        self.active_enemy = -1  # どの敵が出現している？
        self.active_enemy_image = None  # 出現している敵の画像
        self.turn = 0  # 誰の番？
        self.msg = None
        self.wait_time = 100
        self.ememy_attack_damage = None  # 敵の与ダメージ保管用
        self.player_attack_damage = None  # 味方の与ダメージ保管用

    # battle_statusの状態ごとに実行する関数を選ぶ関数
    def battle_trigger(self):
        pygame.key.set_repeat(100, 100)
        if game_status == "BATTLE" and self.battle_status == "INIT":
            self.battle_init()
            self.battle_show_info()
            self.show_enemy()
        elif game_status ==  "BATTLE" and self.battle_status == "COMMAND":
            self.battle_show_info()
            self.battle_command_wnd()
            self.battle_command_select()
            self.show_enemy()
        elif game_status == "BATTLE" and self.battle_status == "PROCESS_PLAYER":
            self.battle_show_info()
            self.battle_process_player()
            self.show_enemy()
        elif game_status == "BATTLE" and self.battle_status == "PROCESS_ENEMY":
            self.battle_show_info()
            self.battle_process_enemy()
            self.show_enemy()
        elif game_status == "BATTLE" and self.battle_status == "WIN":
            self.battle_show_info()
            self.win()
        elif game_status == "BATTLE" and self.battle_status == "GAME_OVER":
            self.battle_show_info()

    # エンカウント時の戦闘準備
    def battle_init(self):
        global pressed_key
        self.msg = "敵が現れた！"
        self.cmd_selecting = 0
        self.turn = 0
        self.active_enemy = self.enemy.enemy_list[0]  # エネミーリストから戦う敵のステータスを取得
        self.active_enemy_hp = self.enemy.enemy_list[0][3]  # エネミーリストからHPを取得
        self.active_enemy_mp = self.enemy.enemy_list[0][4]  # エネミーリストからMPを取得
        self.active_enemy_image = pygame.image.load(self.active_enemy[9])

        for event in pygame.event.get(KEYDOWN):
            if event.key == K_SPACE:
                self.battle_status = "COMMAND"
                pygame.time.wait(self.wait_time)

    # バトル時の情報ウィンドウ(下部)
    def battle_show_info(self):
        pygame.draw.rect(screen, (0, 0, 0), (0, height-100, width, 100))
        pygame.draw.rect(screen, (255, 255, 255), (0, height-100, width, 100), 3)
        msg_text = self.fonts.info_font.render(self.msg, True, (255, 255, 255))
        screen.blit(msg_text, (50, height-50-11))

    def battle_command_wnd(self):
        fight_cmd_color = (127, 127, 127)
        run_cmd_color = (127, 127, 127)
        if self.cmd_selecting == 0:
            fight_cmd_color = (255, 255, 255)
        elif self.cmd_selecting == 1:
            run_cmd_color = (255, 255, 255)
        pygame.draw.rect(screen, (0, 0, 0), (0, 116, 120, height-216))
        pygame.draw.rect(screen, (255, 255, 255), (0, 116, 120, height-216), 3)
        fight_cmd = self.fonts.cmd_font.render("たたかう", True, fight_cmd_color)
        screen.blit(fight_cmd, (116/2 - fight_cmd.get_width()/2, 116 + 30))
        run_cmd = self.fonts.cmd_font.render("にげる", True, run_cmd_color)
        screen.blit(run_cmd, (116/2 - fight_cmd.get_width()/2, 116 + 30 * 2))
        self.battle_command_select()

    def battle_command_select(self):
        # if (self.battle_status == "COMMAND":
        pygame.time.wait(self.wait_time)
        self.msg = "コマンドを選択"
        global pressed_key
        for event in pygame.event.get(KEYDOWN):
            if event.key == K_w:
                self.cmd_selecting = 0
            elif event.key == K_s:
                self.cmd_selecting = 1
            elif event.key == K_SPACE:
                if self.cmd_selecting == 0:
                    self.battle_status = "PROCESS_PLAYER"
                elif self.cmd_selecting == 1:
                    run_away_dice = random.randint(0, 2)  # 逃走の抽選
                    if run_away_dice == 0:  # 逃走に成功したら
                        global game_status
                        game_status = "DUNGEON"
                    else:
                        self.battle_status = "PROCESS_ENEMY"

    def battle_process_player(self):  # 味方の攻撃
        self.player_attack_damage = self.companions.companion_list[self.turn][5] - self.active_enemy[6]
        if self.player_attack_damage < 1:
            self.player_attack_damage = 1
        self.msg = self.companions.companion_list[self.turn][0] + "の攻撃！" + self.active_enemy[0] + "に" + str(self.player_attack_damage) + "のダメージ！"
        for event in pygame.event.get(KEYDOWN):
            if event.key == K_SPACE:
                self.active_enemy_hp -= self.player_attack_damage
                if self.active_enemy_hp < 0:
                    self.battle_status = "WIN"
                else:
                    self.battle_status = "PROCESS_ENEMY"

    def battle_process_enemy(self):  # 敵の攻撃
        self.ememy_attack_damage = self.active_enemy[5] - self.companions.companion_list[self.turn][6]  # 与ダメージの計算
        if self.ememy_attack_damage < 1:
            self.ememy_attack_damage = 1
        self.msg = "敵の攻撃！" + self.companions.companion_list[self.turn][0] + "に" + str(self.ememy_attack_damage) + "のダメージ！"
        for event in pygame.event.get(KEYDOWN):
            if event.key == K_SPACE:
                self.companions.companion_hp[self.turn] -= self.ememy_attack_damage
                if self.turn != 3:
                    self.turn += 1;
                elif self.turn == 3:  # 全員にターンが巡ってきたら
                    self.turn = 0  # 最初の人に戻る
                if self.companions.companion_hp[self.turn]== 0 and self.turn != 3:  # 4番目ではない次の番の人が死んでいたら
                    self.turn += 1  # その番をスキップ
                elif self.companions.companion_hp[self.turn] == 0 and self.turn == 3: # 4番目が死んでいたら
                    self.turn = 0  # 1番目へ
                if self.companions.companion_hp[0] == 0 and self.companions.companion_hp[1] == 0:
                    if self.companions.companion_hp[2] == 0 and self.companions.companion_hp[3] == 0:  # 全滅したら
                        self.battle_status = "GAME_OVER"  # ゲームオーバーへ
                elif self.companions.companion_hp[0] != 0 and self.companions.companion_hp[1] != 0:
                    if self.companions.companion_hp[2] != 0 and self.companions.companion_hp[3] != 0:
                        self.battle_status = "COMMAND" # 生存ならコマンドへ

    def show_enemy(self):   # 敵の画像を画面に表示するメソッド
        global screen
        screen.blit(self.active_enemy_image, (width/2, height/2))

    def win(self):  #勝利後ダンジョンへ戻すメソッド
        self.msg = "プレイヤーの勝利！"
        for event in pygame.event.get(KEYDOWN):
            if event.key == K_SPACE:
                global game_status
                game_status = "DUNGEON"

    def game_over(self):
        self.msg = "ゲームオーバー！Escキーで終了します。"

class Fonts:
    def __init__(self):
        self.info_font = pygame.font.Font("data/fonts/rounded-x-mplus-2p-regular.ttf", 22)
        self.cmd_font = pygame.font.Font("data/fonts/rounded-x-mplus-2p-regular.ttf", 18)



def main():
    pygame.init()
    global pressed_key
    pressed_key = pygame.key.get_pressed()
    global screen
    screen = pygame.display.set_mode(SCREEN_SIZE)
    pygame.display.set_caption(u"Dungeon Visitor")
    player = Player()
    enemy = Enemy()
    companions = Companion()
    graphic = Graphic()
    fonts = Fonts()
    info = Info(fonts, player, companions, map)
    battle = Battle(fonts, companions, enemy)
    map11 = Map(player, graphic, fonts, battle, "1-1.dmap")

    while True:
        screen.fill((223, 223, 223))
        pressed_key = pygame.key.get_pressed()
        map11.trigger()
        info.draw_info()
        battle.battle_trigger()
        pygame.display.update()
        for event in pygame.event.get():
            if event.type == QUIT:
                pygame.quit()
                sys.exit()
            if event.type == KEYDOWN:
                if event.key == K_ESCAPE:
                    pygame.quit()
                    sys.exit()

if __name__ == "__main__":
    main()