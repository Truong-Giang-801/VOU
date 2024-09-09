class User {
  String id;
  String name;
  String password;
  String? email;
  String? phoneNumber;
  String role;
  String lockout;

  User({
    required this.id,
    required this.name,
    required this.password,
    this.email,
    this.phoneNumber,
    required this.role,
    required this.lockout,
  });

  factory User.fromJson(Map<String, dynamic> json) {
    return User(
      id: json['id'] ?? '',
      name: json['name'] ?? '',
      password: json['password'] ?? '',
      email: json['email'],
      phoneNumber: json['phoneNumber'],
      role: json['role'] ?? 'defaultRole',
      lockout: json['lockout'] ?? 'false',
    );
  }
}
