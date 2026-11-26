export type User = {
    id: string;
    name: string;
    email: string;
    imageUrl: string;

};

export type SignUpParams = {
    name: string;
    email: string;
    password: string;
    confirmPassword: string;
    checkout?: boolean;
    dialogId: string;
};

export type SignInParams = {
    email: string;
    password: string;
    checkout?: boolean;
    dialogId: string;
};