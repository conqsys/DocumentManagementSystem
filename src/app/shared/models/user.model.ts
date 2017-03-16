export class UserInfoModel {
  id: number = 0;
  email: string = '';
  userName: string = 'No Users/queues';
  password: string = '';
  enabled: boolean = true;
  phoneNumber: string = '';
  roleId: number = 0;
  accountId: number = 0;
  coordinatorId: number = 0;
  createdDate: Date;
  createdBy: string = '';
  modifiedDate: Date;
  modifiedBy: string = '';
  confirmPassword: string = '';
  isPasswordReset: boolean = false;
  groupUsers: Array<GroupModel> = [];
  userId: number = 0;
}
export class UserModel extends UserInfoModel {
  label: string;
  value: number;
}
export class RoleModel extends UserModel {
  name: string = '--Please Select--';
}

export class CoordinatorsModel extends UserModel {
  userName: string = '--Please Select--';
  id: number = 0;
}

export class ClientModel extends UserModel {
  clientName: string = '--Please Select--';
}

export class GroupModel extends UserModel {
  groupId: number = 0;
  name: string = '';
}





