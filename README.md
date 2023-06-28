# Dog-Knight
![](https://anya-1308928365.cos.ap-nanjing.myqcloud.com/blog/QQ图片20230628222123.png)

## 项目介绍
本项目是一个基于 Unity 的 3D-RPG 类型的游戏 DEMO，主要用于积累和验证游戏开发中常用的技术实现，并帮助熟悉 Unity 引擎。

在该游戏中玩家将扮演小狗骑士白手起家，通过打败各路小怪来收集武器道具，以此来击败最终 BOSS。

## 运行环境
- Unity 2020.3.46f1c1

## 游戏操作
- View: 玩家将以第三人称视角操作小狗骑士
- Move: 通过鼠标点击目标地点指示人物的移动
- Rotate: 按下 ```A``` 或 ```D``` 控制视角的左右方向
- Scroll: 通过鼠标滚轮控制视角的缩放
- Save: 按下 ```S``` 保存当前进度
- Load: 按下 ```L``` 加载最新的存档
- Quit: 按下 ```ESC``` 退出程序

## 游戏截图
![背包系统](https://anya-1308928365.cos.ap-nanjing.myqcloud.com/blog/QQ图片20230628230355.png)

![Boss](https://anya-1308928365.cos.ap-nanjing.myqcloud.com/blog/QQ图片20230628230733.png)

## 实现的主要功能

### 场景切换系统
    - [x] 同场景传送门
    - [x] 不同场景传送门
    - [x] 场景切换的淡入与淡出
### 数值系统（ScriptableObject）
    - [x] 人物状态数值
    - [x] 人物攻击数值
    - [x] 武器攻击数值/物品增益数值
### 战斗系统
    - [x] 人物暴击时特殊动画
    - [x] 敌人 AI 状态机
    - [x] Boss 抛掷石块命中时的碎石以及击退效果
### 存档系统（PlayerPrefs + Json）
    - [x] 存档保存
    - [x] 存档加载
### 背包系统
    - [x] 物品拖拽
    - [x] 快捷栏物品使用
    - [x] 武器栏交互
    - [x] 人物状态栏实时动画（RenderTexture）

