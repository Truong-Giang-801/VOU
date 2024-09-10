class UserBrand {
  final int brandId;
  final String userId;

  UserBrand({
    required this.brandId,
    required this.userId,
  });

  // Convert a UserBrandDto instance into a Map object
  Map<String, dynamic> toJson() {
    return {
      'BrandId': brandId,
      'UserID': userId,
    };
  }

  // Convert a Map object into a UserBrandDto instance
  factory UserBrand.fromJson(Map<String, dynamic> json) {
    return UserBrand(
      brandId: json['BrandId'],
      userId: json['UserID'],
    );
  }
}
