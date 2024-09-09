import 'package:flutter/material.dart';
import 'package:vou_web/class/user.dart';
import 'package:vou_web/api_handler/user_api_handler.dart';
import 'package:vou_web/pages/add_user_page.dart';
import 'package:vou_web/pages/edit_user_page.dart';
class ManageAccountPage extends StatefulWidget {
  @override
  _ManageAccountPageState createState() => _ManageAccountPageState();
}

class _ManageAccountPageState extends State<ManageAccountPage> {
  late Future<List<User>> _userList;

  @override
  void initState() {
    super.initState();
    _loadUserData();
  }

  void _loadUserData() {
    _userList = getUserData();
  }

  // Sort users by role, then by email, then by phone number
  List<User> _sortUsers(List<User> users) {
    users.sort((a, b) {
      int roleComparison = a.role.compareTo(b.role);
      if (roleComparison != 0) return roleComparison;
      int emailComparison = (a.email ?? '').compareTo(b.email ?? '');
      if (emailComparison != 0) return emailComparison;
      return (a.phoneNumber ?? '').compareTo(b.phoneNumber ?? '');
    });
    return users;
  }

  @override
  Widget build(BuildContext context) {
    return Scaffold(
      appBar: AppBar(
        title: Text(
          'Quản lý tài khoản',
          style: TextStyle(
            color: Colors.white,
            fontWeight: FontWeight.bold,
            fontSize: 24,
          ),
        ),
        backgroundColor: Colors.cyan, // Dark blue background
        foregroundColor: Colors.white, // White text
        centerTitle: true, // Center-align the title
        elevation: 8, // Add shadow effect
        actions: [
          IconButton(
            icon: Icon(Icons.add),
            onPressed: () async {
              final result = await Navigator.push(
                context,
                MaterialPageRoute(builder: (context) => AddUserPage()),
              );
              
              // Check if result is true, indicating that a new user was added
              if (result == true) {
                setState(() {
                  _loadUserData(); // Refresh the user list
                });
              }
            },
          ),
        ],
        leading: InkWell(
          onTap: () {
            Navigator.pop(context);
          },
          child: Icon(
            Icons.arrow_back_ios,
            color: Colors.black54,
          ),
        ),
      ),
      body: FutureBuilder<List<User>>(
        future: _userList,
        builder: (context, snapshot) {
          if (snapshot.connectionState == ConnectionState.waiting) {
            return Center(child: CircularProgressIndicator());
          } else if (snapshot.hasError) {
            return Center(child: Text('Không load được dữ liệu'));
          } else if (snapshot.hasData && snapshot.data!.isEmpty) {
            return Center(child: Text('Không tìm thấy user nào tồn tại'));
          } else if (snapshot.hasData) {
            List<User> sortedUsers = _sortUsers(snapshot.data!);
            return ListView.builder(
              itemCount: sortedUsers.length,
              itemBuilder: (context, index) {
                var user = sortedUsers[index];
                return Card(
                  margin: EdgeInsets.symmetric(vertical: 8, horizontal: 16),
                  child: ListTile(
                    title: Text(
                      '${user.email?.isEmpty ?? true ? 'Chưa có email' : user.email} - ${user.phoneNumber?.isEmpty ?? true ? 'Chưa có sđt' : user.phoneNumber}',
                      style: TextStyle(fontWeight: FontWeight.bold),
                    ),
                    subtitle: Column(
                      crossAxisAlignment: CrossAxisAlignment.start,
                      children: [
                        Text('Tên: ${user.name}'),
                        Text('Quyền hạn: ${user.role}'),
                        Text('Trạng thái: ${user.lockout}'),
                      ],
                    ),
                    trailing: Row(
                      mainAxisSize: MainAxisSize.min,
                      children: [
                        Switch(
                          value: user.lockout == 'Activated',
                          onChanged: (bool newValue) {
                            setState(() {
                              user.lockout = newValue ? 'Activated' : 'Locked';
                              userDeActivate(user: user);
                            });
                          },
                        ),
                        IconButton(
                          icon: Icon(Icons.edit),
                          onPressed: () async {
                            final result = await Navigator.push(
                                    context,
                                    MaterialPageRoute(builder: (context) => EditUserPage(user: user)),
                                  );
 
                                    setState(() {
                                      _loadUserData(); // Refresh the user list
                                    });
                                  
                            // Handle edit user action
                          },
                        ),
                        IconButton(
                          icon: Icon(Icons.delete),
                          onPressed: () {
                            deleteUserUI(user);
                          },
                        ),
                      ],
                    ),
                  ),
                );
              },
            );
          } else {
            return Center(child: Text('Unknown error occurred'));
          }
        },
      ),
    );
  }
  
  void deleteUserUI(User user) async {
    try {
      await deleteUser(user: user);
      // Fetch the updated list of users
      final updatedUsers = await getUserData();

      // Update the state with the new list
      setState(() {
        _userList = Future.value(updatedUsers);
      });
    } catch (error) {
      print('Error deleting user: $error');
      // Optionally show an error message to the user
    }
  }
}
