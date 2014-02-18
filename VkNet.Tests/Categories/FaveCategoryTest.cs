﻿using System;
using System.Collections.ObjectModel;
using Moq;
using NUnit.Framework;
using VkNet.Categories;
using VkNet.Model;
using VkNet.Utils;

namespace VkNet.Tests.Categories
{
    [TestFixture]
    public class FaveCategoryTest
    {
        private FaveCategory GetMockedFaveCategory(string url, string json)
        {
            var mock = new Mock<IBrowser>();
            mock.Setup(m => m.GetJson(url.Replace('\'', '"'))).Returns(json);

            return new FaveCategory(new VkApi(){AccessToken = "token", Browser = mock.Object});
        }

        [Test]
        public void GetUsers_OneItem()
        {
            const string url = "https://api.vk.com/method/fave.getUsers?count=3&offset=1&access_token=token&v=5.5";
            const string json =
            @"{
                    'response': {
                      'count': 2,
                      'items': [
                        {
                          'id': 1,
                          'first_name': 'Павел',
                          'last_name': 'Дуров'
                        }
                      ]
                    }
                  }";

            FaveCategory cat = GetMockedFaveCategory(url, json);

            ReadOnlyCollection<User> users = cat.GetUsers(3, 1);

            users.ShouldNotBeNull();
            users.Count.ShouldEqual(1);
            users[0].Id.ShouldEqual(1);
            users[0].FirstName.ShouldEqual("Павел");
            users[0].LastName.ShouldEqual("Дуров");
        }

        [Test]
        public void GetPhotos_NormalCase()
        {
            const string url = "https://api.vk.com/method/fave.getPhotos?count=3&offset=1&access_token=token&v=5.5";
            const string json =
            @"{
                    'response': {
                      'count': 3,
                      'items': [
                        {
                          'id': 263113261,
                          'album_id': 136592355,
                          'owner_id': 1,
                          'photo_75': 'http://cs9591.vk.me/u00001/136592355/s_47267f71.jpg',
                          'photo_130': 'http://cs9591.vk.me/u00001/136592355/m_dc54094a.jpg',
                          'photo_604': 'http://cs9591.vk.me/u00001/136592355/x_3216ccc1.jpg',
                          'photo_807': 'http://cs9591.vk.me/u00001/136592355/y_e10ee835.jpg',
                          'photo_1280': 'http://cs9591.vk.me/u00001/136592355/z_a8fd75ba.jpg',
                          'photo_2560': 'http://cs9591.vk.me/u00001/136592355/w_62aef149.jpg',
                          'text': '',
                          'date': 1307628890
                        },
                        {
                          'id': 319770573,
                          'album_id': -7,
                          'owner_id': -25397178,
                          'user_id': 100,
                          'photo_75': 'http://cs310923.vk.me/v310923070/c28b/VEtf7pX6MXM.jpg',
                          'photo_130': 'http://cs310923.vk.me/v310923070/c28c/cjCqKn_EGxE.jpg',
                          'photo_604': 'http://cs310923.vk.me/v310923070/c28d/IFtj16H-KwI.jpg',
                          'width': 604,
                          'height': 530,
                          'text': '',
                          'date': 1390533904,
                          'post_id': 88997
                        }
                      ]
                    }
                  }";

            FaveCategory cat = GetMockedFaveCategory(url, json);

            ReadOnlyCollection<Photo> photos = cat.GetPhotos(3, 1);

            photos.ShouldNotBeNull();
            photos.Count.ShouldEqual(2);

            photos[0].Id.ShouldEqual(263113261);
            photos[0].AlbumId.ShouldEqual(136592355);
            photos[0].OwnerId.ShouldEqual(1);
            photos[0].Photo75.ShouldEqual(new Uri("http://cs9591.vk.me/u00001/136592355/s_47267f71.jpg"));
            photos[0].Photo130.ShouldEqual(new Uri("http://cs9591.vk.me/u00001/136592355/m_dc54094a.jpg"));
            photos[0].Photo604.ShouldEqual(new Uri("http://cs9591.vk.me/u00001/136592355/x_3216ccc1.jpg"));
            photos[0].Photo807.ShouldEqual(new Uri("http://cs9591.vk.me/u00001/136592355/y_e10ee835.jpg"));
            photos[0].Photo1280.ShouldEqual(new Uri("http://cs9591.vk.me/u00001/136592355/z_a8fd75ba.jpg"));
            photos[0].Photo2560.ShouldEqual(new Uri("http://cs9591.vk.me/u00001/136592355/w_62aef149.jpg"));
            photos[0].Text.ShouldEqual("");
            photos[0].CreateTime.ShouldEqual(new DateTime(2011, 6, 9, 18, 14, 50));

            photos[1].Id.ShouldEqual(319770573);
            photos[1].AlbumId.ShouldEqual(-7);
            photos[1].OwnerId.ShouldEqual(-25397178);
            photos[1].UserId.ShouldEqual(100);
            photos[1].Photo75.ShouldEqual(new Uri("http://cs310923.vk.me/v310923070/c28b/VEtf7pX6MXM.jpg"));
            photos[1].Photo130.ShouldEqual(new Uri("http://cs310923.vk.me/v310923070/c28c/cjCqKn_EGxE.jpg"));
            photos[1].Photo604.ShouldEqual(new Uri("http://cs310923.vk.me/v310923070/c28d/IFtj16H-KwI.jpg"));
            photos[1].Width.ShouldEqual(604);
            photos[1].Height.ShouldEqual(530);
            photos[1].Text.ShouldEqual("");
            photos[1].PostId.ShouldEqual(88997);
            photos[1].CreateTime.ShouldEqual(new DateTime(2014, 1, 24, 7, 25, 4));
        }

        [Test]
        public void GetVideos_NormalCase()
        {
            const string url = "https://api.vk.com/method/fave.getVideos?count=3&offset=1&access_token=token&v=5.5";
            const string json =
            @"{
                    'response': {
                      'count': 2,
                      'items': [
                        {
                          'id': 164841344,
                          'owner_id': 1,
                          'title': 'This is SPARTA',
                          'duration': 16,
                          'description': '',
                          'date': 1366495075,
                          'views': 215502,
                          'comments': 2559,
                          'photo_130': 'http://cs12761.vk.me/u5705167/video/s_df53315c.jpg',
                          'photo_320': 'http://cs12761.vk.me/u5705167/video/l_00c6be47.jpg'
                        }
                      ]
                    }
                  }";

            Assert.Fail("undone");
        }

        [Test]
        public void GetPosts_NotExtended()
        {
            const string url = "https://api.vk.com/method/fave.getPosts?count=3&offset=1&access_token=token&v=5.5";
            const string json =
            @"{
                    'response': {
                      'count': 3,
                      'items': [
                        {
                          'id': 45611,
                          'from_id': 1,
                          'to_id': 1,
                          'date': 1390260904,
                          'post_type': 'post',
                          'text': 'ВКонтакте взял новую высоту — 60 миллионов человек за сутки.',
                          'attachments': [
                            {
                              'type': 'photo',
                              'photo': {
                                'id': 320624027,
                                'album_id': -7,
                                'owner_id': 1,
                                'photo_75': 'http://cs7004.vk.me/c540101/v540101001/945b/6JwHSc5wLGg.jpg',
                                'photo_130': 'http://cs7004.vk.me/c540101/v540101001/945c/xxlEPKAyYXM.jpg',
                                'photo_604': 'http://cs7004.vk.me/c540101/v540101001/945d/jvCKTR8CAHg.jpg',
                                'photo_807': 'http://cs7004.vk.me/c540101/v540101001/945e/UUCwCY799wQ.jpg',
                                'width': 609,
                                'height': 556,
                                'text': '',
                                'date': 1390260965,
                                'access_key': '82b124d82eba43d66d'
                              }
                            }
                          ],
                          'post_source': {
                            'type': 'vk'
                          },
                          'comments': {
                            'count': 0,
                            'can_post': 0
                          },
                          'likes': {
                            'count': 81167,
                            'user_likes': 1,
                            'can_like': 0,
                            'can_publish': 1
                          },
                          'reposts': {
                            'count': 4364,
                            'user_reposted': 0
                          }
                        },
                        {
                          'id': 88997,
                          'from_id': -25397178,
                          'to_id': -25397178,
                          'date': 1390533904,
                          'post_type': 'post',
                          'text': 'Комплекс лучших упражнений на каждую группу мышц \n\nСПИНА \n- Подтягивания 4 подхода 8-12 повторений \n- Становая тяга 3-4 подхода 10-12 повторений \n\n- Тяга (Т-грифа или штанги в наклоне) 3-4 подхода 10-12 повторений \n- Горизонтальная тяга в блочном тренажере 4 подхода 10-12 повторений \n- Шраги со штангой 4 подхода 10-12 повторений \n\nГРУДЬ \n- Жим лёжа (штанги или гантелей) 4 подхода 10-12 повторений \n- Жим на наклонной (штанги или гантелей) 3-4 подхода 10-12 повторений \n- Отжимания на брусьях 4 подхода до отказа \n\nНОГИ \n- Приседания 4 подхода 10-12 повторений \n- Жим ногами 3-4 подхода 8-12 повторений \n- Разгибания ног в тренажере 4 подхода 10-12 повторений \n- Подъём на носки стоя 4 подхода 15 повторений \n- Подъём на носки сидя 3-4 подхода 12-15 повторений \n\nРУКИ \n- Жим лёжа узким хватом 4 подхода 10-12 повторений \n- Отжимания на брусьях 3-4 подхода 8-12 повторений (с доп весом) \n- Подъём штанги на бицепс 4 подхода 10-12 повторений \n- Молот 3-4 подхода 8-12 повторений \n- Армейский жим 3-4 подхода 8-12 повторений \n- Жим сидя 3-4 подхода 8-12 повторений\n- Разведение гантелей в наклоне 3-4 подхода 8-12 повторений \n\nПРЕСС \n- Скручивания 4 подхода 15-20 повторений \n- Косые скручивания 3-4 подхода 12-20 повторений \n- Подъём ног в висе 3-4 подхода 8-15 повторений\n\n#спорт@strog_pocan',
                          'attachments': [
                            {
                              'type': 'photo',
                              'photo': {
                                'id': 319770573,
                                'album_id': -7,
                                'owner_id': -25397178,
                                'user_id': 100,
                                'photo_75': 'http://cs310923.vk.me/v310923070/c28b/VEtf7pX6MXM.jpg',
                                'photo_130': 'http://cs310923.vk.me/v310923070/c28c/cjCqKn_EGxE.jpg',
                                'photo_604': 'http://cs310923.vk.me/v310923070/c28d/IFtj16H-KwI.jpg',
                                'width': 604,
                                'height': 530,
                                'text': '',
                                'date': 1390533904,
                                'post_id': 88997,
                                'access_key': 'bab83089cd5ffeb0f8'
                              }
                            }
                          ],
                          'post_source': {
                            'type': 'api'
                          },
                          'comments': {
                            'count': 0,
                            'can_post': 0
                          },
                          'likes': {
                            'count': 1397,
                            'user_likes': 1,
                            'can_like': 0,
                            'can_publish': 1
                          },
                          'reposts': {
                            'count': 565,
                            'user_reposted': 0
                          }
                        }
                      ]
                    }
                  }";

            Assert.Fail("undone");
        }

        [Test]
        public void GetPosts_Extended()
        {
            const string url = "https://api.vk.com/method/fave.getPosts?count=3&offset=1&extended=1&access_token=token&v=5.5";
            const string json =
            @"{
                    'response': {
                      'count': 3,
                      'items': [
                        {
                          'id': 45611,
                          'from_id': 1,
                          'to_id': 1,
                          'date': 1390260904,
                          'post_type': 'post',
                          'text': 'ВКонтакте взял новую высоту — 60 миллионов человек за сутки.',
                          'attachments': [
                            {
                              'type': 'photo',
                              'photo': {
                                'id': 320624027,
                                'album_id': -7,
                                'owner_id': 1,
                                'photo_75': 'http://cs7004.vk.me/c540101/v540101001/945b/6JwHSc5wLGg.jpg',
                                'photo_130': 'http://cs7004.vk.me/c540101/v540101001/945c/xxlEPKAyYXM.jpg',
                                'photo_604': 'http://cs7004.vk.me/c540101/v540101001/945d/jvCKTR8CAHg.jpg',
                                'photo_807': 'http://cs7004.vk.me/c540101/v540101001/945e/UUCwCY799wQ.jpg',
                                'width': 609,
                                'height': 556,
                                'text': '',
                                'date': 1390260965,
                                'access_key': '82b124d82eba43d66d'
                              }
                            }
                          ],
                          'post_source': {
                            'type': 'vk'
                          },
                          'comments': {
                            'count': 0,
                            'can_post': 0
                          },
                          'likes': {
                            'count': 81168,
                            'user_likes': 1,
                            'can_like': 0,
                            'can_publish': 1
                          },
                          'reposts': {
                            'count': 4364,
                            'user_reposted': 0
                          }
                        },
                        {
                          'id': 88997,
                          'from_id': -25397178,
                          'to_id': -25397178,
                          'date': 1390533904,
                          'post_type': 'post',
                          'text': 'Комплекс лучших упражнений на каждую группу мышц \n\nСПИНА \n- Подтягивания 4 подхода 8-12 повторений \n- Становая тяга 3-4 подхода 10-12 повторений \n\n- Тяга (Т-грифа или штанги в наклоне) 3-4 подхода 10-12 повторений \n- Горизонтальная тяга в блочном тренажере 4 подхода 10-12 повторений \n- Шраги со штангой 4 подхода 10-12 повторений \n\nГРУДЬ \n- Жим лёжа (штанги или гантелей) 4 подхода 10-12 повторений \n- Жим на наклонной (штанги или гантелей) 3-4 подхода 10-12 повторений \n- Отжимания на брусьях 4 подхода до отказа \n\nНОГИ \n- Приседания 4 подхода 10-12 повторений \n- Жим ногами 3-4 подхода 8-12 повторений \n- Разгибания ног в тренажере 4 подхода 10-12 повторений \n- Подъём на носки стоя 4 подхода 15 повторений \n- Подъём на носки сидя 3-4 подхода 12-15 повторений \n\nРУКИ \n- Жим лёжа узким хватом 4 подхода 10-12 повторений \n- Отжимания на брусьях 3-4 подхода 8-12 повторений (с доп весом) \n- Подъём штанги на бицепс 4 подхода 10-12 повторений \n- Молот 3-4 подхода 8-12 повторений \n- Армейский жим 3-4 подхода 8-12 повторений \n- Жим сидя 3-4 подхода 8-12 повторений\n- Разведение гантелей в наклоне 3-4 подхода 8-12 повторений \n\nПРЕСС \n- Скручивания 4 подхода 15-20 повторений \n- Косые скручивания 3-4 подхода 12-20 повторений \n- Подъём ног в висе 3-4 подхода 8-15 повторений\n\n#спорт@strog_pocan',
                          'attachments': [
                            {
                              'type': 'photo',
                              'photo': {
                                'id': 319770573,
                                'album_id': -7,
                                'owner_id': -25397178,
                                'user_id': 100,
                                'photo_75': 'http://cs310923.vk.me/v310923070/c28b/VEtf7pX6MXM.jpg',
                                'photo_130': 'http://cs310923.vk.me/v310923070/c28c/cjCqKn_EGxE.jpg',
                                'photo_604': 'http://cs310923.vk.me/v310923070/c28d/IFtj16H-KwI.jpg',
                                'width': 604,
                                'height': 530,
                                'text': '',
                                'date': 1390533904,
                                'post_id': 88997,
                                'access_key': 'bab83089cd5ffeb0f8'
                              }
                            }
                          ],
                          'post_source': {
                            'type': 'api'
                          },
                          'comments': {
                            'count': 0,
                            'can_post': 0
                          },
                          'likes': {
                            'count': 1397,
                            'user_likes': 1,
                            'can_like': 0,
                            'can_publish': 1
                          },
                          'reposts': {
                            'count': 565,
                            'user_reposted': 0
                          }
                        }
                      ],
                      'profiles': [
                        {
                          'id': 1,
                          'first_name': 'Павел',
                          'last_name': 'Дуров',
                          'sex': 2,
                          'screen_name': 'durov',
                          'photo_50': 'http://cs7004.vk.me/c7003/v7003079/374b/53lwetwOxD8.jpg',
                          'photo_100': 'http://cs7004.vk.me/c7003/v7003563/359e/Hei0g6eeaAc.jpg',
                          'online': 0
                        },
                        {
                          'id': 234695118,
                          'first_name': 'Ruslan',
                          'last_name': 'Davydov',
                          'sex': 2,
                          'screen_name': 'davydov.ruslan',
                          'photo_50': 'http://vk.com/images/camera_c.gif',
                          'photo_100': 'http://vk.com/images/camera_b.gif',
                          'online': 1
                        }
                      ],
                      'groups': [
                        {
                          'id': 25397178,
                          'name': 'Мужские мысли',
                          'screen_name': 'strog_pocan',
                          'is_closed': 0,
                          'type': 'page',
                          'is_admin': 0,
                          'is_member': 0,
                          'photo_50': 'http://cs409122.vk.me/v409122070/b992/iQ5ct6z-V3Y.jpg',
                          'photo_100': 'http://cs409122.vk.me/v409122070/b991/9IJpXZTZbuk.jpg',
                          'photo_200': 'http://cs409122.vk.me/v409122070/b98e/0uLRcwvxKQI.jpg'
                        }
                      ]
                    }
                  }";

            Assert.Fail("undone");
        }
    }
}