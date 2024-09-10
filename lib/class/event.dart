class Event {
  final int id;
  final int brandId; // Ensure this is present
  final int gameId;
  final String name;
  final String img;
  final int numberOfVoucher;
  final DateTime dateCreated;
  final DateTime dateUpdated;

  Event({
    required this.id,
    required this.brandId, // Ensure this is set correctly
    required this.gameId,
    required this.name,
    required this.img,
    required this.numberOfVoucher,
    required this.dateCreated,
    required this.dateUpdated,
  });

  factory Event.fromJson(Map<String, dynamic> json) {
    return Event(
      id: json['id'],
      brandId: json['brandId'],
      gameId: json['gameId'],
      name: json['name'],
      img: json['img'],
      numberOfVoucher: json['numberOfVoucher'],
      dateCreated: DateTime.parse(json['dateCreated']),
      dateUpdated: DateTime.parse(json['dateUpdated']),
    );
  }

  Map<String, dynamic> toJson() {
    return {
      'id': id,
      'brandId': brandId, // Ensure this is included
      'gameId': gameId,
      'name': name,
      'img': img,
      'numberOfVoucher': numberOfVoucher,
      'dateCreated': dateCreated.toIso8601String(),
      'dateUpdated': dateUpdated.toIso8601String(),
    };
  }
}
