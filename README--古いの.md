# ���[�h�~�[ #

Unity�Ń��C�u�����Ƃ��Ďg���ꍇ�̐ݒ���@�̐����Ȃǁ�

### What is this repository for? ###

* Quick summary
* Version
* [Learn Markdown](https://bitbucket.org/tutorials/markdowndemo)

### �ݒ���@ ###

* �\�[�X�R�[�h�� Unity �p�ɐ؂�ւ�����@�B  
Unity�̃��j���[���� [File] - [Build Settings] - [Player Settings] - [Other Settings] - [Scripting Define Symbols] �ɁuUNITY�v�Ɠ��́B  
  
* ��Ճt�@�C��  
����ۂł��BPC�łł̓e�L�X�g�t�@�C���ɋL�q�AUnity�łł̓\�[�X�R�[�h���ɖ��ߍ��݁B�����͌�q�B  
-PC  
--�O���t�@�C�� _auto_joseki_Honshogi(P1).txt �� _auto_joseki_SagareruHiyoko(P1).txt ��Ǐ�������B�t�@�C���͕�������Ă���B  
-Unity  
--[facade/Face_Joseki.cs]�t�@�C�� - [GetKumikomiJoseki()]�֐� �ɁA�O���t�@�C�� �̓��e�𖄂ߍ��ށB  
  
* ���уt�@�C��  
����ۂł��BPC�łł̓e�L�X�g�t�@�C���ɋL�q�AUnity�łł̓\�[�X�R�[�h���ɖ��ߍ��݁B�����͌�q�B  
-PC  
--�O���t�@�C�� _auto_seiseki_Honshogi(P1).txt �� _auto_seiseki_SagareruHiyoko(P1).txt ��Ǐ�������B�t�@�C���͕�������Ă���B  
-Unity  
--[facade/Face_Seiseki.cs]�t�@�C�� - [GetKumikomiSeiseki()]�֐� �ɁA�O���t�@�C�� �̓��e�𖄂ߍ��ށB  
  
* ���֌W�]���l�t�@�C��  
����ۂł��BPC�łł̓e�L�X�g�t�@�C���ɋL�q�AUnity�łł̓\�[�X�R�[�h���ɖ��ߍ��݁B�����͌�q�B  
-PC  
--�O���t�@�C�� _auto_nikoma_Honshogi.txt �� _auto_nikoma_SagareruHiyoko.txt ��Ǐ�������B�t�@�C���͕�������Ă���B  
-Unity  
--[facade/Face_Nikoma.cs]�t�@�C�� - [GetKumikomiNikoma()]�֐� �ɁA�O���t�@�C�� �̓��e�𖄂ߍ��ށB  
  
### �T���v���E�v���O���� ###
shogi34/Program.cs shogi34.Program.Main �R���\�[���E�A�v���P�[�V�����E�v���O����  
�傫�������ĂV�̎菇�ŉ񂵂Ă���B  
-�i�菇�P�j�A�v���P�[�V�����J�n�O�ݒ�  
-�i�菇�Q�j���[�U�[����  
-�i�菇�R�j�l�Ԃ̎��  
-�i�菇�S�j�R���s���[�^�[�̎��  
-�i�菇�T�j������  
-�i�菇�U�j�Q�[���p�̎w����ȊO�̃R�}���h���C�����s  
-�i�菇�V�j�ۑ����ďI��  
  
**�ݒ�  
-�ݒ�̎d��  
--�A�v���P�[�V�����J�n�O  
---Face_Application.Optionlist.���ږ� = �l;  
--�A�v���P�[�V�����J�n��  
---Face_Application.Set( "set ���ږ� �l" );  
  
-�R���s���[�^�[�̎v�l�֘A  
--set P1Com true                        : �v���C���[�P���R���s���[�^�[�Ɏw�����邺��  
--set P2Com true                        : �v���C���[�Q���R���s���[�^�[�Ɏw�����邺��  
--set SaidaiFukasa 3                    : �R���s���[�^�[�̒T���[���̍ő��3�ɐݒ肷�邺��  
--set SikoJikan 4000                    : �R���s���[�^�[�����Ɏv�l���鎞�Ԃ� 4�b �ɐݒ肷�邺��  
--set SikoJikanRandom 1000              : �T������ 0�`0.999�b �͈̔͂Ŏv�l���Ԃ𑽂߂Ɏ�邺��  
--set SennititeKaihi true               : �R���s���[�^�[��������K��������邺��  
-�����I�Ȑݒ�  
--set P1Char HyokatiYusen               : �i�����j�v���C���[�P�̎w����ݒ�B�]���l�D�恙  
--set P1Char SinteYusen                 :         �v���C���[�P�̎w����ݒ�B�V��D�恙  
--set P1Char SinteNomi                  :         �v���C���[�P�̎w����ݒ�B�V��ŗD�恙  
--set P1Char SyorituYusen               :         �v���C���[�P�̎w����ݒ�B�����D�恙  
--set P1Char SyorituNomi                :         �v���C���[�P�̎w����ݒ�B�����ŗD�恙  
--set P1Char TansakuNomi                :         �v���C���[�P�̎w����ݒ�B�T���̂݁�  
--set P2Char ��                         : P1Char �Q�Ɓ�  
--set AspirationFukasa 7                : �A�X�s���[�V�������T�����g���n�߂�[�����i�O�`�O�j  
--set AspirationWindow 300              : �A�X�s���[�V�������T���Ŏg���������i�O�`�O�j  
--set BetaCutPer 100                    : 100%�̊m���Ńx�[�^�E�J�b�g���g������ 0�`100  
--set HanpukuSinkaTansakuTukau true     : �����[���T�����g�������g�����X�|�W�V�����E�e�[�u�����g���K�v���聙  
--set JosekiPer 50                      : 50%�̊m���Œ�Ղ��g������ 0�`100  
--set JosekiRec true                    : ��Ղ̓o�^���s������  
--set Learn true                        : �@�B�w�K���s������  
--set NikomaHyokaKeisu 0.2              : ���֌W�]���l�� 0.2 �{�ɂ��邺��  
--set NikomaGakusyuKeisu 0.01           : ���֌W�]���l�w�K�̒����ʂ� 0.01 �{��  
--set RandomNikoma true                 : �w����Ƀ����_������t���邺��  
--set TranspositionTableTukau true      : �g�����X�|�W�V�����E�e�[�u���g������  
--set UseTimeOver false                 : �v�l���Ԃ̎��Ԑ؂ꔻ��𖳎����邺��  
-�R���\�[���p  
--set P1Name ���ӂ���                 : �v���C���[�P�̕\������ ���ӂ��� �ɕύX��  
--set P2Name ���ӂ���                 : �v���C���[�Q�̕\������ ���ӂ��� �ɕύX��  
--set JohoJikan 3000                    : �ǂ݋ؕ\���� 3000 �~���b�Ԋu�ōs������ �����ŕ\���Ȃ���  
-�Q�[�����[���֘A  
--set SagareruHiyoko true               : �������Ђ悱���[�h�����ʂ̂Ђ悱�͂��Ȃ��Ȃ遙  
-�A���΋ǐݒ�  
--set RandomCharacter true              : �΋ǏI�����ɁACOM�̎w����̐��i��ς��邺��  
--set RandomStart true                  : �J�n�ǖʂ������_���ɂ��邺��  
--set RandomStartTaikyokusya true       : �J�n���������_���ɂ��邺��  
--set RenzokuRandomRule true            : �A���΋ǂ������_���Ƀ��[���ς��Ă�遙  
--set RenzokuTaikyoku true              : �����I������܂ŘA���΋ǂ�����  
* �ˑ��֌W
* �f�[�^�x�[�X
  
* �R�}���h���C��  
  
-�A�v���P�[�V�������I������R�}���h  
-quit  
  
-�R�}���h�ꗗ������R�}���h  
--man  
  
-���݂̐ݒ���m�F����R�}���h  
--set  
  
-�Q�[�����[�h  
--�������R�}���h��ł���[Enter]�L�[��Ō��B�΋ǏI���܂ŃQ�[�����[�h�B  
--�@�R���s���[�^�[���w���Ă���B  
  
-�w����  
--do B3B2  
--��B3�̋��B2�ɓ������B  
--do C2C1+  
--��C2�̋��C2�ɓ������āA����B  
--do H*A2  
--������̂Ђ悱��A2�ɑłB  
--undo B3B2  
--��B2�̋��B3�ɖ߂��B�Ō�� do ��߂��p�r�̂ݗL���B  
  
-�ǖ�  
--hirate  
--�����菉���ǖʂɏ�����  
--ky hanten  
--���Ղ��P�W�O�x��]  
--hyoka  
--���ǂݖ����ŁA���ǖʂ̕]���l��Ԃ��B  
  
-��Ս쐬  
--set JosekiPer 40  
--set SikoJikan 4000  
--set SaidaiFukasa 100  
--set P1Com true  
--set RenzokuTaikyoku true  
--�����Ƃ͎����Œ�Ղ̍쐬���J�n����B  
--��SikoJikan�i�v�l���ԁj �̓~���b�P�ʁB  
  
-COM vs COM �ϐ�  
--set P1Com true  
--set RenzokuTaikyoku true  
  
-MAN vs MAN �΋�  
--set P2Com false  
  
-��Ղ̎����������~�߂����ꍇ  
--set JosekiRec false  
--���܂��� joseki.txt �t�@�C�����A�t�H���_�[�̒���������Ă����B  
  
-��Ղ̎g�p�m��  
--set JosekiPer 100  
--������� 100% �g�p  
--set JosekiPer 0  
--������� �s�g�p  
--���܂��� joseki.txt �t�@�C�����A�t�H���_�[�̒���������Ă������Ƃŕs�g�p�B  
  
-���̑��A����m�F�p�̃R�}���h������������B  
  
* �z�z�菇  
  
### �萔 ###
## �΋ǎ� ##
interfaces/Taikyokusya.cs shogi34.interfaces.Taikyokusya �΋ǎ�  
T1 �΋ǎ҂P  
T2 �΋ǎ҂Q  
  
## �΋ǌ��� ##
interfaces/TaikyokuKekka.cs shogi34.interfaces.TaikyokuKekka �΋ǌ���  
Karappo ���ݒ�  
Taikyokusya1NoKati �΋ǎ҂P�̏���  
Taikyokusya2NoKati �΋ǎ҂Q�̏���  
Hikiwake ��������  
Sennitite ���������i�����j  
  
## �� ##
interfaces/Koma.cs shogi34.interfaces.Koma ���t���̔Տ�̋�  
R �΋ǎ҂P�̂炢����  
Z �΋ǎ҂P�̂���  
K �΋ǎ҂P�̂����  
H �΋ǎ҂P�̂Ђ悱  
N �΋ǎ҂P�̂ɂ�Ƃ�  
r �΋ǎ҂Q�̂炢����  
z �΋ǎ҂Q�̂���  
k �΋ǎ҂Q�̂����  
h �΋ǎ҂Q�̂Ђ悱  
n �΋ǎ҂Q�̂ɂ�Ƃ�  
Kuhaku �󔒏�  
  
interfaces/Koma.cs shogi34.interfaces.MotiKoma ������  
Z �΋ǎ҂P�̂���  
K �΋ǎ҂P�̂����  
H �΋ǎ҂P�̂Ђ悱  
z �΋ǎ҂Q�̂���  
k �΋ǎ҂Q�̂����  
h �΋ǎ҂Q�̂Ђ悱  
  
## ���� ##
interfaces/Komasyurui.cs shogi34.interfaces.Komasyurui ����t���Ȃ��Տ�̋�  
R �炢����  
Z ����  
K �����  
H �Ђ悱  
N �ɂ�Ƃ�  
  
interfaces/Komasyurui.cs shogi34.interfaces.MotiKomasyurui ����t���Ȃ�������
Z ����  
K �����  
H �Ђ悱  
  
## �� ##
interfaces/Masu.cs shogi34.interfaces.Masu  
> [A1] [B1] [C1]  
> [A2] [B2] [C2]  
> [A3] [B3] [C3]  
> [A4] [B4] [C4]  
  
## �w���� ##
interfaces/Sasite.cs shogi34.interfaces.Sasite �w����  
0 ����  

## �w����ԈႢ���R ##
interfaces/Sasite.cs shogi34.interfaces.SasiteMatigaiRiyu �w����ԈႢ���R  
Karappo  �G���[�Ȃ�  
ParameterSyosikiMatigai  �p�����[�^�[�̏������Ԉ���Ă���do�R�}���h  
NaiMotiKomaUti  ��������̂ɋ��ł�����  
BangaiIdo  �ՊO�Ɉړ����悤�Ƃ��������i�O�`�O�j  
TebanKomaNoTokoroheIdo  �����̋�u���Ă���Ƃ���ɁA��𓮂���������  
KomaGaAruTokoroheUti  ��u���Ă���Ƃ���ɁA���ł����񂾂���  
KuhakuWoIdo  �󂫏��ɋ�u���Ă���Ǝv���āA���������Ƃ�������  
AiteNoKomaIdo  ����̋�𓮂������Ƃ���̂́A�C���[�K���E���[�u�������i�O���O�j  
NarenaiNari  �Ђ悱�ȊO���A�ɂ�Ƃ�ɂȂ낤�Ƃ��܂�����  
SonoKomasyuruiKarahaArienaiUgoki  ���̋�̎�ނ���́A���肦�Ȃ����������܂����B  
  
### �f�[�^ ###
## �ǖ� ##
interfaces/Kyokumen.cs shogi34.interfaces.Kyokumen  
Option_Application.Kyokumen �ǖ�  
Option_Application.Kyokumen.Kekka �΋ǌ���  
Option_Application.Kyokumen.Teban ���
Option_Application.Kyokumen.Teme ����ڂ���
Option_Application.Kyokumen.BanjoKomas �Տ�̋�[0]�`[11]
> [ 0] [ 1] [ 2]  
> [ 3] [ 4] [ 5]  
> [ 6] [ 7] [ 8]  
> [ 9] [10] [11]  
Option_Application.Kyokumen.MotiKomas ������̐�[R][Z][H][r][z][h]
  
### ���� ###
## �ǖ� ##
interfaces/Kyokumen.cs shogi34.interfaces.Kyokumen  


### �t�@�C���E�t�H�[�}�b�g ###
## ��Ճt�@�C���E�t�H�[�}�b�g ##
*��  
fen krz/1h1/1H1/ZRK - 1  
B4A3 none 19996 9 123  
B4C3 none 970 13 124  
B3B2 none 747 13 123  
C4C3 none 78 13 124  
fen k1z/1hr/RH1/Z1K - 1  
C4C3 none 19997 9 121  
C4B4 none 19997 9 123  
A3B4 none 187 9 121  
  
*�\��  
�ǖʕ�����  
�w���� none �]���l �[�� �o�[�W����  

## ���уt�@�C���E�t�H�[�}�b�g ##
*��  
fen k1z/1h1/RHr/ZK1 - 1  
B4C4 none 123 0 0 7  
fen k1z/1hr/RH1/Z1K - 1  
C4B4 none 123 0 0 6  
fen krz/1h1/1H1/ZRK - 1  
B4A3 none 123 0 0 6  
B4C3 none 121 7 0 0  
C4C3 none 122 1 0 0  
  
*�\��
�ǖʕ�����
�w���� none �o�[�W���� ������ ���������� ������

## ���֌W�]���l�t�@�C���E�t�H�[�}�b�g ##
*��i�ꕔ�����j
> xxxxxxxxxxxxx     0.00000000     0.00000000     0.00000000  
> -------------  xxxxxxxxxxxxx     0.00000000     0.00000000  
> -------------  -------------  xxxxxxxxxxxxx     0.00000000  
> -------------  -------------  -------------  xxxxxxxxxxxxx  
> -------------  -------------  -------------  -------------  
  
�c����132�~132�̃e�[�u���B�i�Տ�P�Q�O�A���P�Q�j  
����~�P�Q�@�����~�P�Q�@�����~�P�Q�@���Ё~�P�Q�@���Ɂ~�P�Q  
����~�P�Q�@�����~�P�Q�@�����~�P�Q�@���Ё~�P�Q�@���Ɂ~�P�Q  
�����P�@�����Q�@�����P�@�����Q�@���ЂP�@���ЂQ  
�����P�@�����Q�@�����P�@�����Q�@���ЂP�@���ЂQ  
