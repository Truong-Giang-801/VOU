class Event {
  final int id;
  final int brandId;
  final String name;
  final String img;
  final int numberOfVoucher;
  final DateTime dateCreated;
  final DateTime dateUpdated;

  Event({
    required this.id,
    required this.brandId,
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
      'brandId': brandId,
      'name': name,
      'img': img,
      'numberOfVoucher': numberOfVoucher,
      'dateCreated': dateCreated.toIso8601String(),
      'dateUpdated': dateUpdated.toIso8601String(),
    };
  }
}
