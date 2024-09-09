class Game {
  int id;
   String name;
   String img;
   bool allowTrade;
   String instruction;

  Game({
    required this.id,
    required this.name,
    required this.img,
    required this.allowTrade,
    required this.instruction,
  });

  factory Game.fromJson(Map<String, dynamic> json) {
    return Game(
      id: json['id'],
      name: json['name'],
      img: json['img'],
      allowTrade: json['allowTrade'],
      instruction: json['instruction'],
    );
  }

  Map<String, dynamic> toJson() {
    return {
      'id': id,
      'name': name,
      'img': img,
      'allowTrade': allowTrade,
      'instruction': instruction,
    };
  }
}
